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
   
    public partial class Form2 : Form
    {
        public double mouseClickMapX = 0;
        public double mouseClickMapY = 0;
        public int mouseClickX = 0;
        public int mouseClickY = 0; 
        IWorkspace workspace;
        public Form2()
        {
            InitializeComponent();
        }
      

       

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


    }

}


