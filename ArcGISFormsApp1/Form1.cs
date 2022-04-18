using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.SystemUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArcGISFormsApp1
{

    //开发—点击图像显示属性  
    //如果导入的图层无法绘制图像，则可能是因为图层的属性问题，比如设置的单位不是米，或绘制命令被关闭等，需要在arcmap里打开，处理图层的问题
   
    public partial class Form1 : Form
    {
        public double mouseClickMapX = 0;
        public double mouseClickMapY = 0;
        public int mouseClickX = 0;
        public int mouseClickY = 0; 
        IWorkspace workspace;
        public Form1()
        {
            InitializeComponent();
        }
        #region 导入MXD 
        private void ImportMXD_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenMXD = new OpenFileDialog();
            OpenMXD.Title = "打开MXD";
            OpenMXD.InitialDirectory = "E:";
            OpenMXD.Filter = "Map Documents (*.mxd)|*.mxd";
            if (OpenMXD.ShowDialog() == DialogResult.OK)
            {
                string MxdPath = OpenMXD.FileName;
                axMapControl1.LoadMxFile(MxdPath);
            }

        }
        #endregion

        #region 清除所有图层
        private void btnClearLayer_Click(object sender, EventArgs e)
        {
            axMapControl1.Map.ClearLayers();
            axMapControl1.ActiveView.Refresh();
            axTOCControl1.Update();//更新图层控件 
          
        }
        #endregion

        #region 读取Mdb的要素集，要素类，表格数据，栅格数据等数据，并把名称显示在Listbox中
        private void btnImportMDB_Click(object sender, EventArgs e)
        {
            string WsName = SelectMdb();
            List<string> listBoxSource = new List<string>();
            if (WsName != "")
            {
                IWorkspaceFactory workspaceFactory = new AccessWorkspaceFactoryClass();
                workspace = workspaceFactory.OpenFromFile(WsName, 0);

                IEnumDataset enumDataset_workspace = workspace.get_Datasets(esriDatasetType.esriDTAny);
                IDataset dataset_Parent = enumDataset_workspace.Next();
                datalistBox.DataSource = null;
               
                while (dataset_Parent != null)
                {
                  
                    if (dataset_Parent.Type == esriDatasetType.esriDTFeatureClass)//要素类
                    {
                        listBoxSource.Add(dataset_Parent.Name + "-要素类-parent");
                        IFeatureClass featureClass = dataset_Parent as IFeatureClass;//将IDataset强转为IFeatureClass（要素对象） 
                        AddLayer(featureClass);//将要素对象挂载在要素图层上，并显示在地图上
                    }
                    else if (dataset_Parent.Type == esriDatasetType.esriDTFeatureDataset)//要素集
                    {
                        string parentName = dataset_Parent.Name;
                        listBoxSource.Add(parentName + "-要素集-parent");
                        IFeatureDataset featureDataset_workspace = dataset_Parent as IFeatureDataset;

                        IEnumDataset enumDataset_Child = dataset_Parent.Subsets;//取出要素对象的集合
                        IDataset dataset_item = enumDataset_Child.Next();
                        int index = 0;
                        while (dataset_item != null)
                        {
                            listBoxSource.Add(dataset_item.Name + "-要素对象-父：" + parentName+"-" + dataset_item.Type);
                            Console.WriteLine("dataset_item.Type:" + dataset_item.Type);
                            IGeoDataset geoDataset = dataset_item as IGeoDataset; //也可以这样强转
                            IFeatureClass featureClass = dataset_item as IFeatureClass;//将IDataset强转为IFeatureClass（要素对象）
                       
                            AddLayer(featureClass);//将要素对象挂载在要素图层上，并显示在地图上
                            index++;
                            dataset_item = enumDataset_Child.Next();
                        }
                    }
                    else if (dataset_Parent.Type == esriDatasetType.esriDTTable)//数据表
                    {
                        string parentName = dataset_Parent.Name;
                        listBoxSource.Add(parentName + "-数据表-parent");
                        ITable table11_workspace = dataset_Parent as ITable;
                        var count = table11_workspace.RowCount(new QueryFilterClass());
                        Console.WriteLine("数据行数:" + count);

                    }
                    else if (dataset_Parent.Type == esriDatasetType.esriDTRasterDataset)//栅格数据
                    {
                       

                        string parentName = dataset_Parent.Name;
                        listBoxSource.Add(parentName + "-栅格数据-parent");
                    }
                    else
                    {
                        string parentName = dataset_Parent.Name;
                        listBoxSource.Add(parentName + "-parent-" + dataset_Parent.Type.ToString());

                    }

                    dataset_Parent = enumDataset_workspace.Next();
                }
            } 
            datalistBox.DataSource = listBoxSource;
            datalistBox.Refresh();

            #region 刷新地图 
            axMapControl1.ActiveView.Refresh();//全图刷新 
            //axMapControl1.Map.MapScale = axMapControl1.Map.MapScale;
            //axMapControl1.Map.MapScale = 25000;
            Application.DoEvents();
           

            #endregion



        }
        //添加图层
        public void AddLayer(IFeatureClass featureClass)
        { 
            IFeatureLayer featureLayer = new FeatureLayerClass();
            featureLayer.Name = featureClass.AliasName;
            featureLayer.FeatureClass = featureClass;

            ILayerEffects layerEffects = featureLayer as ILayerEffects;
            layerEffects.Transparency = 1;//透明度设置

            IGeoFeatureLayer geoFeatureLayer = featureLayer as IGeoFeatureLayer;
            IFeatureRenderer featRender = geoFeatureLayer.Renderer;
            #region 样式设置 
            if (featRender is ISimpleRenderer)
            {
                ISimpleRenderer simple = featRender as ISimpleRenderer;
                //Symbol一般不会为空，因为有默认值，这里的图层layer是新建的，这里将IFeatureLayer转换为IGeoFeatureLayer，然后取他的Renderer，而Renderer里的Symbol就已经有值了。 
                IFillSymbol symbolFill = simple.Symbol as IFillSymbol;

                #region 获取和设置图层的符号的颜色
                if (symbolFill != null)//可以强转为IFillSymbol，即为填充符号，即面符号
                {
                    RgbColor rgbColor = new RgbColor();
                    rgbColor.RGB = symbolFill.Color.RGB;
                    Color pSymbolColor = Color.FromArgb(rgbColor.Red, rgbColor.Green, rgbColor.Blue);
                    symbolFill.Color = ConvertToArcGisColor(Color.Green);  // 设置图层的符号的颜色
                    //设置图层的符号的边框的颜色,这里直接symbolFill.Outline.Color不好使，必须重新new一个线对象
                    symbolFill.Outline = new SimpleLineSymbolClass() {  Color= ConvertToArcGisColor(Color.Purple), Width = 1 }; 
                      
                    
                }
                else
                {
                    IMarkerSymbol symbolMarker = simple.Symbol as IMarkerSymbol;
                    if (symbolMarker != null)//可以强转为IMarkerSymbol，即为标记符号，即点符号
                    {
                        RgbColor rgbColor = new RgbColor();
                        rgbColor.RGB = symbolMarker.Color.RGB;
                        Color pSymbolColor = Color.FromArgb(rgbColor.Red, rgbColor.Green, rgbColor.Blue);
                        symbolMarker.Color = ConvertToArcGisColor(Color.Red);  // 设置图层的符号的颜色
                    }
                    else
                    {
                        ILineSymbol symbolLine = simple.Symbol as ILineSymbol;
                        if (symbolLine != null)//可以强转为ILineSymbol，即为线符号
                        {
                            RgbColor rgbColor = new RgbColor();
                            rgbColor.RGB = symbolLine.Color.RGB;
                            Color pSymbolColor = Color.FromArgb(rgbColor.Red, rgbColor.Green, rgbColor.Blue);
                            symbolLine.Color = ConvertToArcGisColor(Color.Blue);  // 设置图层的符号的颜色
                        }

                    }
                }
                #endregion

            }
            #endregion
          
            axMapControl1.Map.AddLayer(featureLayer);


        }
        //选择文件数据库
        public string SelectMdb()
        {
            string WsFileName = "";
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Filter = "文件数据库(MDB)|*.mdb";
            DialogResult DialogR = OpenFile.ShowDialog();
            if (DialogR == DialogResult.Cancel)
            {

            }
            else
            {
                WsFileName = OpenFile.FileName;
            }
            return WsFileName;

        }
        #endregion

         
        #region 画点、线、面
        public Operation oprFlag = Operation.Nothing;

        #region 添加图形绘制的单击事件 

        //添加点
        private void btnAddPoint_Click(object sender, EventArgs e)
        {
            oprFlag = Operation.Point;
        } 
        //添加线
        private void btnAddPolyline_Click(object sender, EventArgs e)
        {
            oprFlag = Operation.PolyLine; 
        }
        //添加面
        private void btnAddPolygon_Click(object sender, EventArgs e)
        {
            oprFlag = Operation.Polygon;
        }
        #endregion

        #region 鼠标按下触发画点、线、面
        /// <summary>
        /// axMapContol控件的鼠标单击事件,画完一个图形后，就置为Nothing，这样就可以保证后面使用地图工具时，不一起响应鼠标按下事件
        /// </summary>
        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            mouseClickMapX = e.mapX;
            mouseClickMapY = e.mapY;
            mouseClickX = e.x;
            mouseClickY = e.y;
            //若为添加点的事件
            if (oprFlag == Operation.Point)
            {
                axMapControl1.CurrentTool = null; //axMapControl1控件的当前地图工具为空 【把CurrentControl设为空，否则工具会和自定义的功能函数同时响应鼠标事件】
                Application.DoEvents();
                IPoint pt = new PointClass();
                pt.PutCoords(e.mapX, e.mapY);//设置鼠标单击的坐标为点坐标
                pt.Z = 0;
                AddPoint(pt, 0);

                oprFlag = Operation.Nothing; //点添加完之后结束编辑状态
            }
            //若为添加折线的事件
            else if (oprFlag == Operation.PolyLine)
            {
                axMapControl1.CurrentTool = null; //axMapControl1控件的当前地图工具为空 【把CurrentControl设为空，否则工具会和自定义的功能函数同时响应鼠标事件】
                Application.DoEvents();
                IPoint pt = new PointClass();
                pt.PutCoords(e.mapX, e.mapY); //设置鼠标单击的坐标为点坐标
                 
                //定义集合类型绘制折线的方法
                IGeometry geometry = axMapControl1.TrackLine();
                if (geometry != null && geometry.IsEmpty == false)//有为空的情况
                {
                    AddFeature(geometry, 0); //通过addFeature函数的两个参数, Highways——绘制折线的图层; Geometry——绘制的几何折线
                }
                oprFlag = Operation.Nothing;//折线添加完之后结束编辑状态
            }
            //若为添加面的事件
            else if (oprFlag == Operation.Polygon)
            {

                axMapControl1.CurrentTool = null; //axMapControl1控件的当前地图工具为空 【把CurrentControl设为空，否则工具会和自定义的功能函数同时响应鼠标事件】
                Application.DoEvents();
                var geometry = axMapControl1.TrackPolygon();
                IPointCollection pointColl = geometry as IPointCollection;
                for (int i = 0; i < pointColl.PointCount; i++)
                {
                    IPoint pointTemp = pointColl.Point[i];
                    Console.WriteLine(pointTemp.X +"-"+ pointTemp.Y);
                } 
                
                if (geometry != null && geometry.IsEmpty == false)//有为空的情况
                {
                    //通过AddFeature函数的两个参数, sLayer——绘制折线的图层; pGeometry——绘制几何的图层
                    AddFeature(geometry, 0); //LayerIndex=3 ZRZ
                }
                oprFlag = Operation.Nothing; //面添加完之后结束编辑状态
            }
            //选择并高亮
            else if (oprFlag == Operation.PolygonSelectAndFlash)
            { 
                axMapControl1.CurrentTool = null; //axMapControl1控件的当前地图工具为空 【把CurrentControl设为空，否则工具会和自定义的功能函数同时响应鼠标事件】
                Application.DoEvents();
                var geometry = axMapControl1.TrackPolygon();
                if (geometry != null)//有为空的情况
                {
                    ISelectionEnvironment selectionEnv = new SelectionEnvironmentClass();  // 设置一个新环境
                    selectionEnv.DefaultColor = ConvertToArcGisColor(Color.Red);     // 再改变原来要素的眼神值

                    axMapControl1.Map.SelectByShape(geometry, selectionEnv, false);//高亮选择与几何图形相交的元素
                                                                                   //axMapControl1.FlashShape(geometry, 3, 500, null);//缩放到几何图形并闪烁
                    axMapControl1.ActiveView.Refresh();
                  
                }
                oprFlag = Operation.Nothing; //面添加完之后结束编辑状态
            }
        }
         
        // 获取绘制点的图层, 保存点绘制的函数
        private void AddPoint(IPoint pt, int LayerIndex = 0)
        {

            //得到要添加的图层
            IFeatureLayer pFeatureLayer = axMapControl1.Map.Layer[LayerIndex] as IFeatureLayer;
            if (pFeatureLayer != null)
            {
                //定义一个地物类, 把要编辑的图层转化为定义的地物类
                IFeatureClass featureClass = pFeatureLayer.FeatureClass;
                IDataset dataset = featureClass as IDataset;
                
 
                #region 游标插入 使用IFeatureBuffer

                //获取到IWorkspaceEdit接口，IWorkspaceEdit是编辑必须的接口
                IWorkspaceEdit workspaceEdit = dataset.Workspace as IWorkspaceEdit;

                //开始编辑的两个重要方法，StartEditing第一个参数是是否允许Undo，Redo（重做，撤销），如果是后台数据处理功能一般关系不大
                workspaceEdit.StartEditing(true);
                //构成一个EditOperation有StartEditOperation和StopEditOperation方法，Undo，Redo是针对一个EditOperation的
                workspaceEdit.StartEditOperation();

                //插入要素不止一种方法，此例介绍的是游标插入法，优点是插入速度比较快
                //注意此处的游标是插入游标，而不是之前查询功能的查询结果游标，他们接口是一样的，但是获取方式不同
                IFeatureCursor ftCursor = null;



                //这里加try的原因是，如果编辑时出错，需要调用IWorkspaceEdit的一些方法来回滚修改内容，而且编辑出错几率比一般功能大 
                try
                {
                    //获取插入游标
                    ftCursor = featureClass.Insert(true);

                    //创建featureBuffer  IFeatureBuffer跟IFeature用法差不多，可理解为专用于插入的feature
                    IFeatureBuffer featureBuffer = featureClass.CreateFeatureBuffer();
                    int iShape = featureClass.FindField("SHAPE");
                    IGeometryDef geoDef = featureBuffer.Fields.Field[iShape].GeometryDef;
                    if (geoDef.GeometryType != pt.GeometryType)
                    {
                        MessageBox.Show("图层类型和绘制的图形类型不一致，不能添加。");
                        workspaceEdit.StopEditOperation();
                        workspaceEdit.StopEditing(false);
                        return;
                    }


                    //赋值的方式跟编辑一样 
                    int index = featureBuffer.Fields.FindField("SampleField");
                    featureBuffer.set_Value(index, "518");//注意值类型要一致
                    //修改几何字段的字段值,几何字段只有一个
                    featureBuffer.Shape = pt;

                    //赋值后调用插入游标的InsertFeature方法完成插入一条记录
                    ftCursor.InsertFeature(featureBuffer);
                     
                     
                    //保存编辑 之前的feature.Store()和featureDelete.Delete()都是临时保存，下面两句才是真实的保存
                    workspaceEdit.StopEditOperation();
                    //参数1是是否保存，false就是不保存，也就是恢复到修改前的状态
                    workspaceEdit.StopEditing(true);
                }
                catch (Exception ex)
                {
                    //编辑出错需要以下两句，大意是数据回滚
                    workspaceEdit.StopEditOperation();
                    workspaceEdit.StopEditing(false); 
                    throw ex;
                }
                finally
                {
                    //释放游标
                    if (ftCursor != null)
                        Marshal.ReleaseComObject(ftCursor);

                }

                #endregion
                 

            }
            //屏幕刷新
            this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, pFeatureLayer, null);

        }
        
        /// <summary>
        /// 添加实体对象到地图图层(添加线、面要素)
        /// </summary>
        /// <param name="layerName">图层名称</param>
        /// <param name="geometry">绘制形状(线、面)</param>
        private void AddFeature(IGeometry geometry, int LayerIndex = 0)
        {
           
            //得到要添加的图层
            ILayer layer = axMapControl1.Map.Layer[LayerIndex];
            
            IFeatureLayer featureLayer = layer as IFeatureLayer;
            if (featureLayer != null)
            {
               
                //定义一个要素类, 把要编辑的图层转化为定义的要素类
                IFeatureClass featureClass = featureLayer.FeatureClass; 
                IDataset dataset = featureClass as IDataset;
                
                 
                //先定义一个编辑的工作空间, 然后将其转化为数据集, 最后转化为编辑工作空间
                IWorkspaceEdit w = dataset.Workspace as IWorkspaceEdit;
                //定义游标 查找到最后一条记录, 游标指向该记录后再进行插入操作
                IFeatureCursor ftCursor = featureClass.Search(null, true);
              
                //开始插入新的实体对象(插入对象要使用Insert游标)
                try
                {
                    //开始事务操作
                    w.StartEditing(true);
                    //开始编辑
                    w.StartEditOperation();

                    //在内存创建一个用于暂时存放编辑数据的要素(FeatureBuffer)
                    IFeatureBuffer featureBuffer = featureClass.CreateFeatureBuffer();


                    //开始插入新的实体对象(插入对象要使用Insert游标)
                    ftCursor = featureClass.Insert(true);
                    try
                    {
                        //验证图层的类型和画的图像的类型是否一致 
                        
                        int iShape = featureClass.FindField("SHAPE");
                        IGeometryDef geoDef = featureBuffer.Fields.Field[iShape].GeometryDef;
                        if (geoDef.GeometryType!= geometry.GeometryType)
                        {
                            MessageBox.Show("图层类型和绘制的图形类型不一致，不能添加。");
                            w.StopEditOperation();
                            w.StopEditing(false);
                            return;
                        }
                        //向缓存游标的Shape属性赋值
                        featureBuffer.Shape = geometry;
                        
                    }
                    catch (COMException ex)
                    {
                        MessageBox.Show("绘制的几何图形超出了边界！");
                        w.StopEditOperation();
                        w.StopEditing(false);
                        return;
                    }
                    //判断:几何图形是否为多边形
                    if (geometry.GeometryType.ToString() == "esriGeometryPolygon")
                    {
                        IPolygon polygon = geometry as IPolygon;
                       
                        double lengthValue = polygon.Length; //获取面的周长 
                        double areaValue = (polygon as IArea).Area; //获取面的面积

                        int index = featureBuffer.Fields.FindField("SampleField");//新建的图层 
                        featureBuffer.set_Value(index, "2213");//注意值类型要一致
                    }
                   
                    object featureOID = ftCursor.InsertFeature(featureBuffer);//返回objectID简称OID
                                                                              //保存实体
                    ftCursor.Flush();

                    //保存编辑 之前的feature.Store()和featureDelete.Delete()都是临时保存，下面两句才是真实的保存
                    w.StopEditOperation();
                    //参数1是是否保存，false就是不保存，也就是恢复到修改前的状态
                    w.StopEditing(true);
                }
                catch (Exception ex)
                {
                    //编辑出错需要以下两句，大意是数据回滚
                    w.StopEditOperation();
                    w.StopEditing(false);
                    throw ex;
                }
                finally
                {
                    //释放游标
                    if (ftCursor != null)
                        Marshal.ReleaseComObject(ftCursor);

                }
               
                axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, layer, null);
            }
            else
            {
                MessageBox.Show("未发现" + featureLayer.Name + "图层");
            }
        }

        #endregion
        #endregion

         
        #region 添加【面】图像元素
        private void btnAddPolygon_Custom_Click(object sender, EventArgs e)
        {
            PolygonElementClass polygonElement = new PolygonElementClass();//用于显示的元素
            IPolygon polygon = new PolygonClass();
            IPointCollection pointColl = polygon as IPointCollection;

            //注意：生成面时点集要求第一个点做坐标和最后一个点的坐标一样，也可理解为同一个点添加了两次，否则会出错 
            pointColl.AddPoint(new PointClass() { X = 40606907.9294919, Y = 4303881.578201917 });
            pointColl.AddPoint(new PointClass() { X = 40608226.009079367, Y = 4303886.6870375276 });
            pointColl.AddPoint(new PointClass() { X = 40608113.614695936, Y = 4302941.5524496138 });
            pointColl.AddPoint(new PointClass() { X = 40606907.9294919, Y = 4303881.578201917 });
              
           
            //确定面的形状 
            polygonElement.Geometry = polygon;//将特定元素类型定义为标准几何元素
             
            //创建面的外框线
            ISimpleLineSymbol pSimpleLineSymbol = new SimpleLineSymbolClass();//线型
            pSimpleLineSymbol.Color = ConvertToArcGisColor(Color.Gray); 
            pSimpleLineSymbol.Width = 5;

            //创建样式 
            ISimpleFillSymbol simpleFillSymbol = new SimpleFillSymbolClass();
            simpleFillSymbol.Color = ConvertToArcGisColor(Color.Gold); 
            simpleFillSymbol.Outline = pSimpleLineSymbol;
            simpleFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;//.esriSFSSolid;//.esriSFSNull; esriSFSHollow


            IFillShapeElement pFillShapeElement = (IFillShapeElement)polygonElement;
            pFillShapeElement.Symbol = simpleFillSymbol;//将面与样式绑定


            #region 这样添加不显示图形，类似于选择高亮
            //IMap map = axMapControl1.Map;
            //IActiveView actView = map as IActiveView;  
            //actView.ScreenDisplay.SetSymbol(simpleFillSymbol as ISymbol);
            //actView.ScreenDisplay.DrawPolygon(polygon);
            #endregion

            //向地图图形容器添加几何元素
            IGraphicsContainer graphicsContainer = axMapControl1.Map as IGraphicsContainer;
            graphicsContainer.AddElement(polygonElement, 0);
         
            //graphicsContainer.DeleteElement(polygonElement);//删除指定图像

            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, polygonElement, null);
            axMapControl1.ActiveView.Refresh(); 

        }
        #endregion

        #region 清空【面】图像元素
        private void btnClearPolygon_Custom_Click(object sender, EventArgs e)
        {
           
            IGraphicsContainer graphicsContainer = axMapControl1.Map as IGraphicsContainer;
            graphicsContainer.DeleteAllElements(); 
            axMapControl1.ActiveView.Refresh();
        }
        #endregion


        #region 允许地图可以画几何图形，并高亮相交元素  
        private void btnSelectPolygon_Click(object sender, EventArgs e)
        {
            oprFlag = Operation.PolygonSelectAndFlash;
        }
        #endregion

        #region 清空高亮选择 
        private void btnClearSelectPolygon_Click(object sender, EventArgs e)
        {
            axMapControl1.Map.ClearSelection();
            axMapControl1.ActiveView.Refresh();
            oprFlag = Operation.Nothing; //结束编辑状态
        }
        #endregion

        #region 图层控件(axTOCControl1)右键打开数据表格

        public ILayer pGlobalFeatureLayer = null;
        //图层鼠标按下
        private void axTOCControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.ITOCControlEvents_OnMouseDownEvent e)
        {
            if (axMapControl1.LayerCount > 0)//不论左右键点击，都触发
            {
                esriTOCControlItem pItem = new esriTOCControlItem();
                pGlobalFeatureLayer = new FeatureLayerClass();
                IBasicMap pBasicMap = new MapClass();
                object pOther = new object();

                object pIndex = new object();
                //通过HitTest方法获取到FeatureLayer，并保持到全局变量，为后面的打开属性表使用
                axTOCControl1.HitTest(e.x, e.y, ref pItem, ref pBasicMap, ref pGlobalFeatureLayer, ref pOther, ref pIndex);
                
            }
            if (e.button == 2)//右键时 e.button等于2
            {
                contextMenuStrip1.Show(axTOCControl1, e.x, e.y);
            }

        }
        //打开表格
        private void 打开表格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pGlobalFeatureLayer != null)
            {
                FormTable Ft = new FormTable(pGlobalFeatureLayer as IFeatureLayer);
                Ft.Show();
            }

        }

        #endregion 

        #region 鼠标移动 地图(axMapControl1)下方显示坐标 
        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            try
            {
                toolStripStatusLabel1.Text = string.Format("{0},{1}  {2}", e.mapX.ToString("#######.##"), e.mapY.ToString("#######.##"), axMapControl1.MapUnits.ToString().Substring(4));
            }
            catch
            { }

        }
        #endregion

        #region 缓存查询，点击地图某一个点后，在这个点扩展500米，并高亮选择图形
        private void btnCacheQuery_Click(object sender, EventArgs e)
        {

            IMap map = axMapControl1.Map;
            IActiveView actView = map as IActiveView;
             
            IPoint pt = actView.ScreenDisplay.DisplayTransformation.ToMapPoint(mouseClickX, mouseClickY);
            ITopologicalOperator pTopo = pt as ITopologicalOperator;
            IGeometry pGeo = pTopo.Buffer(500);

          
            ISimpleFillSymbol simpleFillSymbol = new SimpleFillSymbolClass();
            simpleFillSymbol.Color = ConvertToArcGisColor(Color.Gold); 
            ISymbol symbol = simpleFillSymbol as ISymbol;
        
             
            map.SelectByShape(pGeo, null, false); 
            axMapControl1.FlashShape(pGeo, 1, 2, symbol);  //闪动1次
            axMapControl1.ActiveView.Refresh();
        }
        #endregion

        #region 新建要素类
        private void btnAddTable_Click(object sender, EventArgs e)
        {
            if(workspace == null)
            {
                MessageBox.Show("请先导入mdb");
                return;
            }
             
            IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
            ISpatialReference spatialReference = axMapControl1.ActiveView.FocusMap.SpatialReference;
            
            IGeometryDefEdit pGeoDef = new GeometryDefClass();
            IGeometryDefEdit pGeoDefEdit = pGeoDef as IGeometryDefEdit;
            ////创建面要素对象
            //pGeoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
            //创建线要素对象
            //pGeoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolyline; 
            ////创建点要素对象
            pGeoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPoint;



            pGeoDefEdit.SpatialReference_2 = spatialReference;

            //定义一个字段集合对象

            IFields pFields = new FieldsClass();
            IFieldsEdit pFieldsEdit = (IFieldsEdit)pFields;

            //定义单个的字段

            IField pField = new FieldClass();
            IFieldEdit pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "SHAPE";

            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
            pFieldEdit.GeometryDef_2 = pGeoDef;
            pFieldsEdit.AddField(pField); 

            //定义单个的字段，并添加到字段集合中 
            pField = new FieldClass(); pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "SampleField";

            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldsEdit.AddField(pField);

            //定义单个的字段，并添加到字段集合中 
            pField = new FieldClass(); pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "SLM10";

            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldsEdit.AddField(pField);

            //定义单个的字段，并添加到字段集合中 
            pField = new FieldClass(); pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "SLM20";

            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldsEdit.AddField(pField);

            //定义单个的字段，并添加到字段集合中 
            pField = new FieldClass(); pFieldEdit = (IFieldEdit)pField;
            pFieldEdit.Name_2 = "SLM40";

            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pFieldsEdit.AddField(pField);


         

            IWorkspace2 workspace2 = workspace as IWorkspace2;
            
            CreateFeatureClass(workspace2, null, "test_dian", pFields);
            axMapControl1.ActiveView.Refresh();//全图刷新 
            axTOCControl1.Update();//更新图层控件 
             

        }
        /// <summary>
        /// 在workspace中创建featureclass，即创建一个含有图像的表
        /// </summary>
        /// <param name="workspace">IWorkspace2接口</param>
        /// <param name="featureDataset">IFeatureDataset接口或传入Null。</param>
        /// <param name="featureClassName">FeatureClass的名字</param>
        /// <param name="fields">IFields字段数组，就定义普通字段即可，objectId和shape字段会默认生成。shape字段存储几何图形</param>
        /// <param name="CLSID">UID或Null，如“esriGeoDatabase.Feature”</param> 
        /// <param name="esriFeatureType">featureClass的类型，默认是esriFeatureType.esriFTSimple，表示面要素对象</param>
        /// <param name="CLSEXT">UID或Null，（如果要在创建要素类时引用类扩展，则这是类扩展）</param>
        /// <param name="strConfigKeyword">空字符串或SDE数据库的表明，例如："myTable"或""</param>

        /// <returns>返回一个IFeatureClass或Null</returns> 

        ///（1）如果“featureClassName”已存在于工作区中，则返回该要素类。  
        ///（2）如果为“featureDataset”参数传入IFeatureDataset，则要素类将在数据集中创建。如果“featureDataset”没有传入任何内容参数将在工作空间中创建要素类。  
        ///（3）在数据集中创建要素类时，将从数据集对象继承空间参照。 
        ///（4）如果为“字段”集合提供了IFields接口，它将用于创建表格。如果为“fields”集合提供了Nothing值，则将使用方法中的默认值。  
        ///（5）“strConfigurationKeyword”参数允许应用程序控制底层RDBMS的物理布局，例如，在Oracle中，strConfigKeyword控制表空间创建表、初始值、扩展和其他属性。 
        /// ArcSDE的数据管理员用户（ArcSDE data administrator）会根据“strConfigurationKeyword”创建一个ArcSDE的实例。 
        /// 使用IWorkspaceConfiguration的ConfigurationKeywords属性，可获得可用关键字的列表。有关配置的更多信息关键字，请参阅ArcSDE文档。
        /// 不使用ArcSDE表格时，请传入空值，例如""。

        public ESRI.ArcGIS.Geodatabase.IFeatureClass CreateFeatureClass(
            ESRI.ArcGIS.Geodatabase.IWorkspace2 workspace, 
            ESRI.ArcGIS.Geodatabase.IFeatureDataset featureDataset,
            System.String featureClassName, 
            ESRI.ArcGIS.Geodatabase.IFields fields, 
            ESRI.ArcGIS.Geodatabase.esriFeatureType esriFeatureType = ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple,
            ESRI.ArcGIS.esriSystem.UID CLSID = null, 
            ESRI.ArcGIS.esriSystem.UID CLSEXT = null, 
            System.String strConfigKeyword = null
          )
        {
            if (featureClassName == "") return null; // name was not passed in 

            ESRI.ArcGIS.Geodatabase.IFeatureClass featureClass;
            ESRI.ArcGIS.Geodatabase.IFeatureWorkspace featureWorkspace = (ESRI.ArcGIS.Geodatabase.IFeatureWorkspace)workspace; // Explicit Cast

            if (workspace.get_NameExists(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass, featureClassName)) //feature class with that name already exists 
            {
                featureClass = featureWorkspace.OpenFeatureClass(featureClassName);
                return featureClass;
            }

            // assign the class id value if not assigned
            if (CLSID == null)
            {
                CLSID = new ESRI.ArcGIS.esriSystem.UIDClass();
                CLSID.Value = "esriGeoDatabase.Feature";
            }

            ESRI.ArcGIS.Geodatabase.IObjectClassDescription objectClassDescription = new ESRI.ArcGIS.Geodatabase.FeatureClassDescriptionClass();

            //如果字段集合为空，则插入默认值
            if (fields == null)
            {
                // create the fields using the required fields method
                fields = objectClassDescription.RequiredFields;
                ESRI.ArcGIS.Geodatabase.IFieldsEdit fieldsEdit = (ESRI.ArcGIS.Geodatabase.IFieldsEdit)fields; // Explicit Cast
                ESRI.ArcGIS.Geodatabase.IField field = new ESRI.ArcGIS.Geodatabase.FieldClass();

                // create a user defined text field
                ESRI.ArcGIS.Geodatabase.IFieldEdit fieldEdit = (ESRI.ArcGIS.Geodatabase.IFieldEdit)field; // Explicit Cast

                // setup field properties
                fieldEdit.Name_2 = "SampleField";
                fieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString;
                fieldEdit.IsNullable_2 = true;
                fieldEdit.AliasName_2 = "Sample Field Column";
                fieldEdit.DefaultValue_2 = "test";
                fieldEdit.Editable_2 = true;
                fieldEdit.Length_2 = 100;

                // add field to field collection
                fieldsEdit.AddField(field);
                fields = (ESRI.ArcGIS.Geodatabase.IFields)fieldsEdit; // Explicit Cast
            }


            System.String strShapeField = "";
            // locate the shape field
            for (int j = 0; j < fields.FieldCount; j++)
            {
                if (fields.get_Field(j).Type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeGeometry)
                {
                    strShapeField = fields.get_Field(j).Name;
                }
            }

            // Use IFieldChecker to create a validated fields collection.
            ESRI.ArcGIS.Geodatabase.IFieldChecker fieldChecker = new ESRI.ArcGIS.Geodatabase.FieldCheckerClass();
            ESRI.ArcGIS.Geodatabase.IEnumFieldError enumFieldError = null;
            ESRI.ArcGIS.Geodatabase.IFields validatedFields = null;
            fieldChecker.ValidateWorkspace = (ESRI.ArcGIS.Geodatabase.IWorkspace)workspace;
            fieldChecker.Validate(fields, out enumFieldError, out validatedFields);

            // The enumFieldError enumerator can be inspected at this point to determine 
            // which fields were modified during validation.
            // finally create and return the feature class
            if (featureDataset == null)// if no feature dataset passed in, create at the workspace level
            {
                featureClass = featureWorkspace.CreateFeatureClass(featureClassName, validatedFields, CLSID, CLSEXT, ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple, strShapeField, strConfigKeyword);
            }
            else
            {
                featureClass = featureDataset.CreateFeatureClass(featureClassName, validatedFields, CLSID, CLSEXT, ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple, strShapeField, strConfigKeyword);
            }
            return featureClass;
        }

        #endregion

        #region 删除指定名称的类（表）
        private void btnDelTable_Click(object sender, EventArgs e)
        {
            if (workspace == null)
            {
                MessageBox.Show("请先导入mdb");
                return;
            }

            IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
            IFeatureClass featureClass = featureWorkspace.OpenFeatureClass("test518");
            IDataset datset = featureClass as IDataset;
            datset.Delete(); 

        }
        #endregion

        #region 删除全部的类（表）

        #endregion
        private void btnDelAllTable_Click(object sender, EventArgs e)
        {
           
            string WsName = SelectMdb(); 
            if (WsName != "")
            {
                IWorkspaceFactory workspaceFactory = new AccessWorkspaceFactoryClass();
                IWorkspace workspaceDel = workspaceFactory.OpenFromFile(WsName, 0);
                IEnumDataset enumDataset_workspace = workspaceDel.get_Datasets(esriDatasetType.esriDTAny);
                IDataset dataset_Parent = enumDataset_workspace.Next();
               
                while (dataset_Parent != null)
                {
                    dataset_Parent.Delete();
                    dataset_Parent = enumDataset_workspace.Next();
                }
            }

        }


        #region 创建要素集
        private void btnAddDataset_Click(object sender, EventArgs e)
        {
            if (workspace == null)
            {
                MessageBox.Show("请先导入mdb");
                return;
            }
            IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
            ISpatialReference spatialReference = axMapControl1.ActiveView.FocusMap.SpatialReference;
            //创建要素集
            featureWorkspace.CreateFeatureDataset("Data2", spatialReference);
        }
        #endregion

        #region 导出为图片
        /// <summary>
        /// 地图输出
        /// </summary>
        /// <param name="map">地图</param>
        /// <param name="outputFile">输出文件路径</param>
        /// <param name="resolution">水平和垂直分辨率</param>
        /// <returns>是否成功</returns>
        public static bool MapExport(IMap map, string outputFile, double resolution)
        {
            IActiveView activeView = map as IActiveView;
            if (activeView == null) return false;

            IExport exporter = null;
            string extension = System.IO.Path.GetExtension(outputFile);
            switch (extension.ToLower())
            {
                case ".jpg":
                    exporter = new ExportJPEGClass() as IExport;
                    break;

                case ".pdf":
                    exporter = new ExportPDFClass() as IExport;
                    break;

                case ".png":
                    exporter = new ExportPNGClass() as IExport;
                    break;

                case ".tif":
                    exporter = new ExportTIFFClass() as IExport;
                    break;

                case ".bmp":
                    exporter = new ExportBMPClass() as IExport;
                    break;

                case ".gif":
                    exporter = new ExportGIFClass() as IExport;
                    break;

                default:
                    throw new Exception("输出图片格式不支持！");

            }

            try
            {
                double screenResolution = activeView.ScreenDisplay.DisplayTransformation.Resolution;

                exporter.ExportFileName = outputFile;
                exporter.Resolution = resolution;

                tagRECT exportRect = new tagRECT();
                exportRect.left = 0;
                exportRect.top = 0;
                exportRect.right = Convert.ToInt32(activeView.ExportFrame.right * (resolution / screenResolution));
                exportRect.bottom = Convert.ToInt32(activeView.ExportFrame.bottom * (resolution / screenResolution));

                IEnvelope pixelBoundsEnv = new EnvelopeClass();
                pixelBoundsEnv.PutCoords(exportRect.left, exportRect.top, exportRect.right, exportRect.bottom);
                exporter.PixelBounds = pixelBoundsEnv;

                int hDC = exporter.StartExporting();
                activeView.Output(hDC, Convert.ToInt32(resolution), ref exportRect, null, null);
                exporter.FinishExporting();

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                exporter.Cleanup();
            }
        }

        private void btnImportIMG_Click(object sender, EventArgs e)
        {
            IMap map = axMapControl1.Map;
            MapExport(map, "A.jpg", 300);
        }
        #endregion

        #region 公共方法
        //使用IMap.SelectFeature方法对地图中的要素进行查询
        void SearchHightlight(IMap _pMap, IFeatureLayer _pFeatureLayer, IQueryFilter _pQuery, bool _Bool)
        {
            IFeatureCursor pFtCursor = _pFeatureLayer.Search(_pQuery, _Bool);
            IFeature pFt = pFtCursor.NextFeature();
            while (pFt != null)
            {
                _pMap.SelectFeature(_pFeatureLayer as ILayer, pFt);
                pFt = pFtCursor.NextFeature();
            }
        }
        /// <summary>
        /// 获取指定名称的图层，从shpfile文件中获取
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="LayerName"></param>
        /// <returns></returns>
        private IFeatureClass GetFeatureClass(string FilePath, string LayerName)
        { 
            IWorkspaceFactory pWks = new ShapefileWorkspaceFactoryClass(); 
            IFeatureWorkspace pFwk = pWks.OpenFromFile(FilePath, 0) as IFeatureWorkspace; 
            IFeatureClass pRtClass = pFwk.OpenFeatureClass(LayerName);

            return pRtClass;
        }
        /// <summary>
        /// 获取指定名称的要素类，从mdb数据库中获取
        /// </summary>
        /// <param name="p_GDBFileName"></param>
        /// <param name="p_FeatureClassName"></param>
        /// <returns></returns>
        private IFeatureClass OpenFeatureClass(string p_GDBFileName, string p_FeatureClassName)
        { 
            IWorkspaceFactory li_WorkspaceFactory = new AccessWorkspaceFactoryClass();
            IWorkspace li_Workspace = li_WorkspaceFactory.OpenFromFile(p_GDBFileName, 0);
            IFeatureWorkspace li_FeatureWorkspace = li_Workspace as IFeatureWorkspace;
            IFeatureClass li_FeatureClass = li_FeatureWorkspace.OpenFeatureClass(p_FeatureClassName);
            return li_FeatureClass;

        }
        //打开文件数据库
        public IWorkspace GetFGDBWorkspace(String _pGDBName)
        {
            IWorkspaceFactory pWsFac = new FileGDBWorkspaceFactoryClass();
            IWorkspace pWs = pWsFac.OpenFromFile(_pGDBName, 0);
            return pWs;
        }
        //打开SDE数据库
        public IWorkspace GetSDEWorkspace(String _pServerIP, String _pInstance, String _pUser, String _pPassword, String _pDatabase, String _pVersion = "SDE.DEFAULT")
        {

            IPropertySet pPropertySet = new  PropertySetClass();
            pPropertySet.SetProperty("SERVER", _pServerIP);
            pPropertySet.SetProperty("INSTANCE", _pInstance);
            pPropertySet.SetProperty("DATABASE", _pDatabase);
            pPropertySet.SetProperty("USER", _pUser);
            pPropertySet.SetProperty("PASSWORD", _pPassword);
            pPropertySet.SetProperty("VERSION", _pVersion);
            IWorkspaceFactory2 workspaceFactory;
            workspaceFactory = (ESRI.ArcGIS.Geodatabase.IWorkspaceFactory2)new ESRI.ArcGIS.DataSourcesGDB.SdeWorkspaceFactoryClass();
            return workspaceFactory.Open(pPropertySet, 0);
        }
        //添加SDE数据库的图层
        public void AddSDELayer(bool ChkSdeLinkModle)
        {
            //定义一个属性 
            IPropertySet Propset = new PropertySetClass();
            if (ChkSdeLinkModle == true) // 采用SDE连接 
            {
                //设置数据库服务器名，服务器所在的IP地址 
                Propset.SetProperty("SERVER", "192.168.2.41");
                //设置SDE的端口，这是安装时指定的，默认安装时"port:5151" 
                Propset.SetProperty("INSTANCE", "port:5151");
                //SDE的用户名 
                Propset.SetProperty("USER", "sa");
                //密码 
                Propset.SetProperty("PASSWORD", "sa");
                //设置数据库的名字,只有SQL Server  Informix 数据库才需要设置 
                Propset.SetProperty("DATABASE", "sde");
                //SDE的版本,在这为默认版本 
                Propset.SetProperty("VERSION", "SDE.DEFAULT");
            }
            else // 直接连接 
            {
                //设置数据库服务器名,如果是本机可以用"sde:sqlserver:." 
                Propset.SetProperty("INSTANCE", "sde:sqlserver:zhpzh");
                //SDE的用户名 
                Propset.SetProperty("USER", "sa");
                //密码 
                Propset.SetProperty("PASSWORD", "sa");
                //设置数据库的名字,只有SQL Server  Informix 数据库才需要设置            
                Propset.SetProperty("DATABASE", "sde");
                //SDE的版本,在这为默认版本 
                Propset.SetProperty("VERSION", "SDE.DEFAULT");
            }
            //定义一个工作空间,并实例化为SDE的工作空间 
            IWorkspaceFactory Fact = new SdeWorkspaceFactoryClass();
            //打开SDE工作空间,并转化为地物工作空间 
            IFeatureWorkspace Workspace = (IFeatureWorkspace)Fact.Open(Propset, 0);
            /*定义一个地物类,并打开SDE中的管点地物类,写的时候一定要写全.如SDE中有一个管点层,你不能写成IFeatureClass Fcls = Workspace.OpenFeatureClass ("管点");这样,一定要写成下边的样子.*/
            IFeatureClass Fcls = Workspace.OpenFeatureClass("sde.dbo.管点");

            IFeatureLayer Fly = new FeatureLayerClass();
            Fly.FeatureClass = Fcls;
            axMapControl1.Map.AddLayer(Fly);
            axMapControl1.ActiveView.Refresh();
        }

        /// <summary>
        /// 将.NET中的Color结构转换至于ArcGIS Engine中的IColor接口
        /// </summary>
        /// <param name="color">.NET中的System.Drawing.Color结构表示ARGB颜色</param>
        /// <returns>ArcGIS Egnine中的IColor接口</returns>
        public static IColor ConvertToArcGisColor(Color color)
        {
            IColor pColor = new RgbColorClass();
            pColor.RGB = color.B * 65536 + color.G * 256 + color.R;
            return pColor;
        }

        private string esriGeometryTypeToString(esriGeometryType geotype)
        {
            string stype = geotype.ToString();

            switch (geotype)
            {
                case esriGeometryType.esriGeometryAny:
                    stype = "任何类型（Any valid geometry）";
                    break;
                case esriGeometryType.esriGeometryBag:
                    stype = "任意几何类型的集合（GeometryBag）";
                    break;
                case esriGeometryType.esriGeometryBezier3Curve:
                    stype = "贝兹曲线（BezierCurve）";
                    break;
                case esriGeometryType.esriGeometryCircularArc:
                    stype = "圆弧（CircularArc）";
                    break;
                case esriGeometryType.esriGeometryEllipticArc:
                    stype = "椭圆弧（EllipticArc）";
                    break;
                case esriGeometryType.esriGeometryEnvelope:
                    stype = "外包（Envelope）";
                    break;
                case esriGeometryType.esriGeometryLine:
                    stype = "线段（Line）";
                    break;
                case esriGeometryType.esriGeometryMultiPatch:
                    stype = "表面几何（MultiPatch）";
                    break;
                case esriGeometryType.esriGeometryMultipoint:
                    stype = "多点（Multipoint）";
                    break;
                case esriGeometryType.esriGeometryNull:
                    stype = "未知类型（Unknown）";
                    break;
                case esriGeometryType.esriGeometryPath:
                    stype = "路径（Path）";
                    break;
                case esriGeometryType.esriGeometryPoint:
                    stype = "点（Point）";
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    stype = "多边形（Polygon）";
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    stype = "多段线（Polyline）";
                    break;
                case esriGeometryType.esriGeometryRay:
                    stype = "射线（Ray）";
                    break;
                case esriGeometryType.esriGeometryRing:
                    stype = "环（Ring）";
                    break;
                case esriGeometryType.esriGeometrySphere:
                    stype = "球体（Sphere）";
                    break;
                case esriGeometryType.esriGeometryTriangleFan:
                    stype = "三角扇形（TriangleFan）";
                    break;
                case esriGeometryType.esriGeometryTriangleStrip:
                    stype = "三角带（TriangleStrip）";
                    break;
                case esriGeometryType.esriGeometryTriangles:
                    stype = "三角形（Triangles）";
                    break;
                default:
                    break;
            }

            return stype;
        }



        #endregion


        //空间查询 第一个参数为面数据,第二个参数为点数据,第三个为输出的表 
        public void StatisticPointCount(IFeatureClass _pPolygonFClass, IFeatureClass _pPointFClass, ITable _pTable)
        {
            IFeatureCursor pPolyCursor = _pPolygonFClass.Search(null, false);
            IFeature pPolyFeature = pPolyCursor.NextFeature();

            while (pPolyFeature != null)
            {
                IGeometry pPolGeo = pPolyFeature.Shape;
                int Count = 0;
                ISpatialFilter spatialFilter = new SpatialFilterClass();
                spatialFilter.Geometry = pPolGeo;
                spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;

                IFeatureCursor pPointCur = _pPointFClass.Search(spatialFilter, false);

                if (pPointCur != null)
                {
                    IFeature pPointFeature = pPointCur.NextFeature();

                    while (pPointFeature != null)
                    {
                        pPointFeature = pPointCur.NextFeature();
                        Count++;
                    }
                }
                if (Count != 0)
                {
                    IRow pRow = _pTable.CreateRow();
                    pRow.set_Value(1, pPolyFeature.get_Value(0));
                    pRow.set_Value(2, Count);
                    pRow.Store();
                }
                pPolyFeature = pPolyCursor.NextFeature();
            }
        }
        /// <summary>
        /// 高亮条件查询结果
        /// </summary>
        /// <param name="where"></param>
        public void SelectAndHighlight(string where= "TYPE='test'")
        {
            IMap pMap = axMapControl1.Map;
            IFeatureLayer pFeaturelayer = pMap.Layer[0] as IFeatureLayer;
            IFeatureSelection pFeatureSelection = pFeaturelayer as IFeatureSelection;
            IQueryFilter pQuery = new QueryFilterClass();
            pQuery.WhereClause = where;//条件从句
            pFeatureSelection.SelectFeatures(pQuery, esriSelectionResultEnum.esriSelectionResultNew, false);
            axMapControl1.ActiveView.Refresh();
        }

        /// <summary>  
        /// 通过IFeature.Delete方法删除要素  
        /// </summary>  
        /// <param name="pFeatureclass">要素类</param>  
        /// <param name="strWhereClause">查询条件</param>  
        public static void DeleteFeatureByIFeature(IFeatureClass pFeatureclass, string strWhereClause)
        {
            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = strWhereClause;
            IFeatureCursor pFeatureCursor = pFeatureclass.Search(pQueryFilter, false);
            IFeature pFeature = pFeatureCursor.NextFeature();
            while (pFeature != null)
            {
                pFeature.Delete();
                pFeature = pFeatureCursor.NextFeature();
            }
        }

        /// <summary>  
        /// 通过IFeatureCursor.DeleteFeature方法删除要素  
        /// </summary>  
        /// <param name="pFeatureclass">要素类</param>  
        /// <param name="strWhereClause">查询条件</param>  
        public static void DeleteFeatureByIFeatureCursor(IFeatureClass pFeatureclass, string strWhereClause)
        {
            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = strWhereClause;
            IFeatureCursor pFeatureCursor = pFeatureclass.Update(pQueryFilter, false);
            IFeature pFeature = pFeatureCursor.NextFeature();
            while (pFeature != null)
            {
                pFeatureCursor.DeleteFeature();
                pFeature = pFeatureCursor.NextFeature();
            }
        }


        /// <summary>  
        /// 通过ITable.DeleteSearchedRows方法删除要素  
        /// </summary>  
        /// <param name="pFeatureclass">要素类</param>  
        /// <param name="strWhereClause">查询条件</param>  
        public static void DeleteFeatureByITable(IFeatureClass pFeatureclass, string strWhereClause)
        {
            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = strWhereClause;
            ITable pTable = pFeatureclass as ITable;
            pTable.DeleteSearchedRows(pQueryFilter);
        }


      
        //axMapControl1.CurrentTool = null;
        ////ControlsMapZoomOutTool
        //axMapControl1.MousePointer = esriControlsMousePointer.esriPointerZoomIn;
      

        private void btnSelectFeature_Click(object sender, EventArgs e)
        {
            axMapControl1.CurrentTool = null;
            ControlsSelectFeaturesTool pTool = new ControlsSelectFeaturesToolClass();
            pTool.OnCreate(axMapControl1.Object);
            axMapControl1.CurrentTool = pTool as ITool;

        }

     
    }

}


