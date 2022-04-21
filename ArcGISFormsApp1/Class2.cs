//using ESRI.ArcGIS.Carto;
//using ESRI.ArcGIS.Controls;
//using ESRI.ArcGIS.DataSourcesFile;
//using ESRI.ArcGIS.DataSourcesGDB;
//using ESRI.ArcGIS.DataSourcesRaster;
//using ESRI.ArcGIS.Display;
//using ESRI.ArcGIS.esriSystem;
//using ESRI.ArcGIS.Geodatabase;
//using ESRI.ArcGIS.Geometry;
//using ESRI.ArcGIS.Output;
//using ESRI.ArcGIS.SystemUI;
//using stdole;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Forms;


//namespace ArcGISFormsApp1
//{

//    //打开类


//    public class clsOpenClass
//    {
//        public static void OpenFeatureClass(AxMapControl MapControl,
//            IFeatureClassName pFcName, ListView listview1)
//        {
//            try
//            {
//                MapControl.Map.ClearLayers();
//                MapControl.SpatialReference = null;
//                IName pName = pFcName as IName;
//                IFeatureClass pFc = pName.Open() as IFeatureClass;

//                listview1.Items.Clear();
//                listview1.Columns.Clear();
//                LoadListView(pFc, listview1);

//                IFeatureCursor pCursor = pFc.Search(null, false);
//                IFeature pfea = pCursor.NextFeature();
//                int j = 0;
//                while (pfea != null)
//                {
//                    ListViewItem lv = new ListViewItem();

//                    for (int i = 0; i < pfea.Fields.FieldCount; i++)
//                    {
//                        string sFieldName = pfea.Fields.get_Field(i).Name;
//                        lv.SubItems.Add(FeatureHelper.GetFeatureValue(pfea, sFieldName).ToString());
//                    }

//                    lv.Tag = pfea;
//                    if (j % 2 == 0)
//                    {
//                        lv.BackColor = System.Drawing.Color.GreenYellow;
//                    }
//                    listview1.Items.Add(lv);
//                    pfea = pCursor.NextFeature();
//                    j++;
//                }
//                LSGISHelper.OtherHelper.ReleaseObject(pCursor);
//                //最后加载图形数据  


//                if (pFcName.FeatureType == esriFeatureType.esriFTRasterCatalogItem)
//                {
//                    ESRI.ArcGIS.Carto.IGdbRasterCatalogLayer pGdbRCLayer = new ESRI.ArcGIS.Carto.GdbRasterCatalogLayerClass();
//                    pGdbRCLayer.Setup(pFc as ITable);
//                    MapControl.Map.AddLayer(pGdbRCLayer as ILayer);
//                }
//                else if ((pFcName.FeatureType == esriFeatureType.esriFTSimple) ||
//                     (pFcName.FeatureType == esriFeatureType.esriFTComplexEdge) ||
//                    (pFcName.FeatureType == esriFeatureType.esriFTComplexJunction) ||
//                    (pFcName.FeatureType == esriFeatureType.esriFTSimpleEdge) ||
//                     (pFcName.FeatureType == esriFeatureType.esriFTSimpleJunction))
//                {

//                    IFeatureLayer pLayer = new FeatureLayerClass();
//                    pLayer.FeatureClass = pFc;
//                    pLayer.Name = (pFc as IDataset).Name;
//                    MapControl.Map.AddLayer(pLayer as ILayer);
//                }
//                else if (pFcName.FeatureType == esriFeatureType.esriFTAnnotation)
//                {
//                    ILayer pLayer = OpenAnnotationLayer(pFc);
//                    pLayer.Name = (pFc as IDataset).Name;
//                    MapControl.Map.AddLayer(pLayer as ILayer);
//                }

//                MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
//            }
//            catch (Exception ex)
//            { }
//        }
//        public static void OpenRasterDataset(AxMapControl MapControl,
//            IRasterDatasetName pRdName, ListView listview1)
//        {
//            MapControl.ClearLayers();
//            MapControl.SpatialReference = null;
//            listview1.Items.Clear();
//            listview1.Columns.Clear();
//            IDatasetName pDsName = pRdName as IDatasetName;
//            string sName = pDsName.Name;

//            IName pName = pRdName as IName;

//            IRasterDataset pRds = pName.Open() as IRasterDataset;
//            IRasterLayer pRL = new RasterLayerClass();
//            pRL.CreateFromDataset(pRds);
//            pRL.Name = sName;
//            MapControl.AddLayer(pRL as ILayer);
//            MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

//        }

//        public static void OpenMosaicDataset(AxMapControl MapControl,
//           IMosaicDatasetName pMdName, ListView listview1)
//        {
//            MapControl.ClearLayers();
//            MapControl.SpatialReference = null;
//            listview1.Items.Clear();
//            listview1.Columns.Clear();
//            IDatasetName pDsName = pMdName as IDatasetName;
//            string sName = pDsName.Name;

//            IName pName = pMdName as IName;

//            IMosaicDataset pMds = pName.Open() as IMosaicDataset;
//            IFeatureClass pFc = pMds.Catalog;
//            listview1.Items.Clear();
//            listview1.Columns.Clear();
//            LoadListView(pFc, listview1);

//            IFeatureCursor pCursor = pFc.Search(null, false);
//            IFeature pfea = pCursor.NextFeature();
//            int j = 0;
//            while (pfea != null)
//            {
//                ListViewItem lv = new ListViewItem();

//                for (int i = 0; i < pfea.Fields.FieldCount; i++)
//                {
//                    string sFieldName = pfea.Fields.get_Field(i).Name;
//                    lv.SubItems.Add(FeatureHelper.GetFeatureValue(pfea, sFieldName).ToString());
//                }

//                lv.Tag = pfea;
//                if (j % 2 == 0)
//                {
//                    lv.BackColor = System.Drawing.Color.GreenYellow;
//                }
//                listview1.Items.Add(lv);
//                pfea = pCursor.NextFeature();
//                j++;
//            }
//            LSGISHelper.OtherHelper.ReleaseObject(pCursor);
//            IMosaicLayer pML = new MosaicLayerClass();
//            pML.CreateFromMosaicDataset(pMds);

//            MapControl.AddLayer(pML as ILayer);
//            MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

//        }

//        public static void OpenTable(AxMapControl MapControl,
//            ITableName pTName, ListView listview1)
//        {
//            try
//            {
//                MapControl.Map.ClearLayers();
//                MapControl.SpatialReference = null;
//                MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, null);
//                IName pName = pTName as IName;
//                ITable pFc = pName.Open() as ITable;

//                listview1.Items.Clear();
//                listview1.Columns.Clear();
//                LoadListView(pFc, listview1);

//                ICursor pCursor = pFc.Search(null, false);
//                IRow pfea = pCursor.NextRow();
//                int j = 0;
//                while (pfea != null)
//                {
//                    ListViewItem lv = new ListViewItem();

//                    for (int i = 0; i < pfea.Fields.FieldCount; i++)
//                    {
//                        string sFieldName = pfea.Fields.get_Field(i).Name;
//                        lv.SubItems.Add(FeatureHelper.GetRowValue(pfea, sFieldName).ToString());
//                    }

//                    lv.Tag = pfea;
//                    if (j % 2 == 0)
//                    {
//                        lv.BackColor = System.Drawing.Color.GreenYellow;
//                    }
//                    listview1.Items.Add(lv);
//                    pfea = pCursor.NextRow();
//                    j++;
//                }
//                LSGISHelper.OtherHelper.ReleaseObject(pCursor);
//            }
//            catch { }
//        }
//        public static void LoadListView(IFeatureClass pFC, ListView listView1)
//        {
//            try
//            {

//                listView1.Columns.Clear();
//                //添加一个空  
//                ColumnHeader columnHeader = new ColumnHeader();

//                listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
//                   columnHeader
//                   });
//                columnHeader.Text = "";

//                for (int i = 0; i < pFC.Fields.FieldCount; i++)
//                {
//                    ColumnHeader columnHeader1 = new ColumnHeader();

//                    listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
//                   columnHeader1
//                   });
//                    IFields pFields = pFC.Fields;

//                    IField pField = pFields.get_Field(i);


//                    columnHeader1.Text = pField.AliasName;


//                }

//            }
//            catch (Exception ex)
//            { }
//        }
//        public static void LoadListView(ITable pFC, ListView listView1)
//        {
//            try
//            {

//                listView1.Columns.Clear();
//                //添加一个空  
//                ColumnHeader columnHeader = new ColumnHeader();

//                listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
//                   columnHeader
//                   });
//                columnHeader.Text = "";

//                for (int i = 0; i < pFC.Fields.FieldCount; i++)
//                {
//                    ColumnHeader columnHeader1 = new ColumnHeader();

//                    listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
//                   columnHeader1
//                   });
//                    IFields pFields = pFC.Fields;

//                    IField pField = pFields.get_Field(i);


//                    columnHeader1.Text = pField.AliasName;


//                }

//            }
//            catch (Exception ex)
//            { }
//        }

//        public static ILayer OpenAnnotationLayer(IFeatureClass pfc)
//        {
//            IFDOGraphicsLayerFactory pfdof = new FDOGraphicsLayerFactoryClass();
//            IFeatureDataset pFDS = pfc.FeatureDataset;
//            IWorkspace pWS = pFDS.Workspace;
//            IFeatureWorkspace pFWS = pWS as IFeatureWorkspace;
//            ILayer pLayer = pfdof.OpenGraphicsLayer(pFWS, pFDS, (pfc as IDataset).Name);
//            return pLayer;
//        }

//    }


//    //创建类



//    public class clsCreateClass
//    {
//        public IFeatureDataset CreateDataset(IWorkspace pWorkspace)
//        {
//            try
//            {
//                if (pWorkspace == null) return null;
//                IFeatureWorkspace aFeaWorkspace = pWorkspace as IFeatureWorkspace;
//                if (aFeaWorkspace == null) return null;
//                DatasetPropertiesForm aForm = new DatasetPropertiesForm();
//                aForm.HignPrecision = LSGISHelper.WorkspaceHelper.HighPrecision(pWorkspace);
//                if (aForm.ShowDialog() == DialogResult.OK)
//                {
//                    string dsName = aForm.FeatureDatasetName;
//                    ISpatialReference aSR = aForm.SpatialReference;
//                    IFeatureDataset aDS = aFeaWorkspace.CreateFeatureDataset(dsName, aSR);
//                    return aDS;
//                }
//            }
//            catch (Exception ex) { }
//            return null;
//        }
//        public IRasterDataset CreateRasterDataset(IWorkspace pWorkspace, string sName
//           )
//        {
//            try
//            {
//                IRasterWorkspaceEx pRWEx = pWorkspace as IRasterWorkspaceEx;
//                IGeometryDef pGDef = new GeometryDefClass();

//                IRasterDataset pRD = pRWEx.CreateRasterDataset(
//                    sName, 3, rstPixelType.PT_CHAR, null, null, null, null);
//            }
//            catch { }
//            return null;
//        }
//        public IFeatureClass CreateFeatureClass(IWorkspace pWorkspace)
//        {
//            if (pWorkspace == null) return null;
//            IFeatureWorkspace aFeaWorkspace = pWorkspace as IFeatureWorkspace;
//            if (aFeaWorkspace == null) return null;
//            IFeatureClass aClass = null;
//            FeatureClassWizard aForm = new FeatureClassWizard();
//            aForm.Workspace = pWorkspace;
//            if (aForm.ShowDialog() == DialogResult.OK)
//            {
//                while (true)
//                {
//                    string className = aForm.FeatureClassName;
//                    string aliasName = aForm.FeatureClassAliasName;
//                    IFields flds = aForm.Fields;
//                    try
//                    {
//                        aClass = aFeaWorkspace.CreateFeatureClass(className, flds
//                            , null, null, esriFeatureType.esriFTSimple, "SHAPE", null);
//                        if (!aliasName.Equals(""))
//                        {
//                            IClassSchemaEdit aClassEdit = aClass as IClassSchemaEdit;
//                            if (aClassEdit != null) aClassEdit.AlterAliasName(aliasName);
//                        }
//                        break;
//                    }
//                    catch (Exception ex)
//                    {
//                        //MessageBox.Show ("错误:\n"+ex.Message ,"新建特性类",  
//                        //    MessageBoxButtons.OK ,MessageBoxIcon.Error );  
//                        LSCommonHelper.MessageBoxHelper.ShowErrorMessageBox(ex, "");
//                    }
//                    aForm = new FeatureClassWizard();
//                    aForm.Workspace = pWorkspace;
//                    aForm.FeatureClassName = className;
//                    aForm.FeatureClassAliasName = aliasName;
//                    aForm.Fields = flds;
//                    if (aForm.ShowDialog() == DialogResult.Cancel) break;
//                }
//            }
//            return aClass;
//        }
//        public IFeatureClass CreateFeatureClass(IFeatureDataset pDS)
//        {
//            if (pDS == null) return null;
//            IFeatureClass aClass = null;

//            FeatureClassWizard aForm = new FeatureClassWizard();
//            aForm.Workspace = (pDS as IDataset).Workspace;
//            IGeoDataset pGDS = pDS as IGeoDataset;
//            if (pGDS != null)
//            {
//                aForm.SpatialReference = pGDS.SpatialReference;
//            }
//            if (aForm.ShowDialog() == DialogResult.OK)
//            {
//                while (true)
//                {
//                    string className = aForm.FeatureClassName;
//                    string aliasName = aForm.FeatureClassAliasName;
//                    IFields flds = aForm.Fields;

//                    try
//                    {
//                        aClass = pDS.CreateFeatureClass(className, flds
//                            , null, null, esriFeatureType.esriFTSimple, "SHAPE", null);
//                        if (!aliasName.Equals(""))
//                        {
//                            IClassSchemaEdit aClassEdit = aClass as IClassSchemaEdit;
//                            if (aClassEdit != null) aClassEdit.AlterAliasName(aliasName);
//                        }
//                        break;
//                    }
//                    catch (Exception ex)
//                    {
//                        LSCommonHelper.MessageBoxHelper.ShowErrorMessageBox(ex, "请选择高精度坐标系");
//                    }
//                    aForm = new FeatureClassWizard();
//                    aForm.Workspace = (pDS as IDataset).Workspace;
//                    aForm.FeatureClassName = className;
//                    aForm.FeatureClassAliasName = aliasName;


//                    aForm.Fields = flds;
//                    if (aForm.ShowDialog() == DialogResult.Cancel) break;
//                }
//            }
//            return aClass;
//        }
//        public ITable CreateTable(IWorkspace pWorkspace)
//        {
//            if (pWorkspace == null) return null;
//            IFeatureWorkspace aFeaWorkspace = pWorkspace as IFeatureWorkspace;
//            if (aFeaWorkspace == null) return null;
//            ITable aTable = null;
//            DataTableWizard aWizard = new DataTableWizard();
//            aWizard.Workspace = pWorkspace;
//            if (aWizard.ShowDialog() == DialogResult.OK)
//            {
//                while (true)
//                {
//                    string tableName = aWizard.TableName;
//                    string aliasName = aWizard.TableAliasName;
//                    IFields flds = aWizard.Fields;
//                    try
//                    {
//                        aTable = aFeaWorkspace.CreateTable(tableName, flds
//                            , null, null, null);

//                        if (!aliasName.Equals(""))
//                        {
//                            IClassSchemaEdit aClassEdit = aTable as IClassSchemaEdit;
//                            aClassEdit.RegisterAsObjectClass("OBJECTID", null);
//                            if (aClassEdit != null) aClassEdit.AlterAliasName(aliasName);
//                        }
//                        break;
//                    }
//                    catch (Exception ex)
//                    {
//                        //MessageBox.Show ("错误:\n"+ex.Message ,"新建表",  
//                        //    MessageBoxButtons.OK ,MessageBoxIcon.Error );  
//                        LSCommonHelper.MessageBoxHelper.ShowErrorMessageBox(ex, "");
//                    }
//                    aWizard = new DataTableWizard();
//                    aWizard.Workspace = pWorkspace;
//                    aWizard.TableName = tableName;
//                    aWizard.TableAliasName = aliasName;
//                    aWizard.Fields = flds;
//                    if (aWizard.ShowDialog() == DialogResult.Cancel) break;
//                }
//            }
//            return aTable;
//        }
//    }


//    //导出类



//    public class clsExportClass
//    {

//        /// <summary>  
//        /// 导出FeatureClass到Shapefile文件  
//        /// </summary>  
//        /// <param name="apFeatureClass"></param>  
//        public static bool ExportFeatureClassToShp(string sPath, IFeatureClass apFeatureClass)
//        {
//            try
//            {
//                string ExportFileShortName = System.IO.Path.GetFileNameWithoutExtension(sPath);
//                if (ExportFileShortName == "")
//                {
//                    ExportFileShortName = LSCommonHelper.OtherHelper.GetRightName((apFeatureClass as IDataset).Name, ".");
//                }
//                string ExportFilePath = System.IO.Path.GetDirectoryName(sPath);
//                if (ExportFilePath == null)
//                {
//                    ExportFilePath = sPath;
//                }
//                //设置导出要素类的参数  
//                IFeatureClassName pOutFeatureClassName = new FeatureClassNameClass();
//                IDataset pOutDataset = (IDataset)apFeatureClass;
//                pOutFeatureClassName = (IFeatureClassName)pOutDataset.FullName;
//                //创建一个输出shp文件的工作空间  
//                IWorkspaceFactory pShpWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
//                IWorkspaceName pInWorkspaceName = new WorkspaceNameClass();
//                pInWorkspaceName = pShpWorkspaceFactory.Create(ExportFilePath, ExportFileShortName, null, 0);

//                //创建一个要素集合  
//                IFeatureDatasetName pInFeatureDatasetName = null;
//                //创建一个要素类  
//                IFeatureClassName pInFeatureClassName = new FeatureClassNameClass();
//                IDatasetName pInDatasetClassName;
//                pInDatasetClassName = (IDatasetName)pInFeatureClassName;
//                pInDatasetClassName.Name = ExportFileShortName;//作为输出参数  
//                pInDatasetClassName.WorkspaceName = pInWorkspaceName;
//                //通过FIELDCHECKER检查字段的合法性，为输出SHP获得字段集合  
//                long iCounter;
//                IFields pOutFields, pInFields;

//                IField pGeoField;
//                IEnumFieldError pEnumFieldError = null;
//                pInFields = apFeatureClass.Fields;
//                IFieldChecker pFieldChecker = new FieldChecker();
//                pFieldChecker.Validate(pInFields, out pEnumFieldError, out pOutFields);
//                //通过循环查找几何字段  
//                pGeoField = null;
//                for (iCounter = 0; iCounter < pOutFields.FieldCount; iCounter++)
//                {
//                    if (pOutFields.get_Field((int)iCounter).Type == esriFieldType.esriFieldTypeGeometry)
//                    {
//                        pGeoField = pOutFields.get_Field((int)iCounter);
//                        break;
//                    }
//                }
//                //得到几何字段的几何定义  
//                IGeometryDef pOutGeometryDef;
//                IGeometryDefEdit pOutGeometryDefEdit;
//                pOutGeometryDef = pGeoField.GeometryDef;
//                //设置几何字段的空间参考和网格  
//                pOutGeometryDefEdit = (IGeometryDefEdit)pOutGeometryDef;
//                pOutGeometryDefEdit.GridCount_2 = 1;
//                pOutGeometryDefEdit.set_GridSize(0, 1500000);

//                //开始导入  
//                IFeatureDataConverter pShpToClsConverter = new FeatureDataConverterClass();
//                pShpToClsConverter.ConvertFeatureClass(pOutFeatureClassName, null, pInFeatureDatasetName, pInFeatureClassName, pOutGeometryDef, pOutFields, "", 1000, 0);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }
//        public static void ExportFeatureClass2Shapefile(IFeatureClassName pFcName)
//        {

//            IName pName = pFcName as IName;
//            IFeatureClass pFc = pName.Open() as IFeatureClass;

//            SaveFileDialog ofd = new SaveFileDialog();
//            ofd.Filter = "Shapefile文件(.shp)|*.shp";
//            if (ofd.ShowDialog() == DialogResult.OK)
//            {
//                string sPath = ofd.FileName;
//                if (ExportFeatureClassToShp(sPath, pFc))
//                {
//                    LSCommonHelper.MessageBoxHelper.ShowMessageBox("导出成功");
//                }
//                LSCommonHelper.MessageBoxHelper.ShowMessageBox("导出失败");
//            }
//        }

//        public static bool ConvertFeatureDataset(IWorkspace sourceWorkspace, IWorkspace targetWorkspace,
//     string nameOfSourceFeatureDataset, string nameOfTargetFeatureDataset)
//        {
//            try
//            {
//                //create source workspace name    
//                IDataset sourceWorkspaceDataset = (IDataset)sourceWorkspace;
//                IWorkspaceName sourceWorkspaceName = (IWorkspaceName)sourceWorkspaceDataset.FullName;
//                //create source dataset name     
//                IFeatureDatasetName sourceFeatureDatasetName = new FeatureDatasetNameClass();
//                IDatasetName sourceDatasetName = (IDatasetName)sourceFeatureDatasetName;
//                sourceDatasetName.WorkspaceName = sourceWorkspaceName;
//                sourceDatasetName.Name = nameOfSourceFeatureDataset;
//                //create target workspace name     
//                IDataset targetWorkspaceDataset = (IDataset)targetWorkspace;
//                IWorkspaceName targetWorkspaceName = (IWorkspaceName)targetWorkspaceDataset.FullName;
//                //create target dataset name    
//                IFeatureDatasetName targetFeatureDatasetName = new FeatureDatasetNameClass();
//                IDatasetName targetDatasetName = (IDatasetName)targetFeatureDatasetName;
//                targetDatasetName.WorkspaceName = targetWorkspaceName;
//                targetDatasetName.Name = nameOfTargetFeatureDataset;
//                //Convert feature dataset       
//                IFeatureDataConverter featureDataConverter = new FeatureDataConverterClass();
//                featureDataConverter.ConvertFeatureDataset(sourceFeatureDatasetName, targetFeatureDatasetName, null, "", 1000, 0);
//                return true;
//            }
//            catch (Exception ex)
//            { return false; }
//        }

//        public static void ExportFeatureDataset2GDB(IDatasetName pDSName, int flag)
//        {
//            FolderBrowserDialog fbd = new FolderBrowserDialog();
//            fbd.Description = "选择保存路径";
//            if (fbd.ShowDialog() == DialogResult.OK)
//            {
//                string sPath = fbd.SelectedPath;
//                string sTemplate = "";
//                if (flag == 0)
//                {
//                    sTemplate = Application.StartupPath + @"\template\pgdb.mdb";
//                    File.Copy(sTemplate, sPath + "\\pgdb.mdb");
//                }
//                else
//                {
//                    sTemplate = Application.StartupPath + @"\template\fgdb.gdb";
//                    FileHelper.CopyDir(sTemplate, sPath + "\\fgdb.gdb");
//                }

//                IName pName = pDSName as IName;
//                string sSrcDSName = pDSName.Name;
//                sSrcDSName = LSCommonHelper.OtherHelper.GetRightName(sSrcDSName, ".");
//                IDataset pDS = pName.Open() as IDataset;
//                IWorkspace pSrcWS = pDS.Workspace;
//                IWorkspace pDesWS = null;
//                if (flag == 0)
//                {
//                    pDesWS = LSGISHelper.WorkspaceHelper.GetAccessWorkspace(sPath + "\\pgdb.mdb");
//                }
//                else
//                {
//                    pDesWS = LSGISHelper.WorkspaceHelper.GetFGDBWorkspace(sPath + "\\fgdb.gdb");
//                }
//                if (ConvertFeatureDataset(pSrcWS, pDesWS, sSrcDSName, sSrcDSName))
//                {
//                    LSCommonHelper.MessageBoxHelper.ShowMessageBox("导出成功");
//                }
//                else
//                {
//                    LSCommonHelper.MessageBoxHelper.ShowMessageBox("导出失败");
//                }
//            }

//        }

//        public static void ExportFeatureDataset2Shapefile(IDatasetName pDSName,
//             TaskMonitor mTaskMonitor)
//        {

//            FolderBrowserDialog fbd = new FolderBrowserDialog();
//            fbd.Description = "选择保存路径";
//            if (fbd.ShowDialog() == DialogResult.OK)
//            {
//                string sPath = fbd.SelectedPath;
//                IName pName = pDSName as IName;
//                IDataset pDS = pName.Open() as IDataset;
//                IFeatureDataset pFDS = pDS as IFeatureDataset;
//                IFeatureClassContainer pFCC = pFDS as IFeatureClassContainer;
//                IFeatureClass pfc = null;
//                mTaskMonitor.EnterWaitState();

//                for (int i = 0; i < pFCC.ClassCount; i++)
//                {
//                    pfc = pFCC.get_Class(i);
//                    mTaskMonitor.TaskCaption = "正在导出第" + (i + 1) + "个" + pfc.AliasName + "图层，共" + pFCC.ClassCount + "个";
//                    mTaskMonitor.TaskProgress = LSCommonHelper.MathHelper.Precent(
//                        0, pFCC.ClassCount, i);
//                    if (ExportFeatureClassToShp(sPath, pfc))
//                    { }
//                }
//                mTaskMonitor.ExitWaitState();
//                LSCommonHelper.MessageBoxHelper.ShowMessageBox("导出完毕");
//            }
//        }

//    }


//    导入类



//    public class clsImportClass
//    {
//        public static void ImportGDB2SDE(IWorkspace pDesWS, int flag)
//        {
//            IWorkspace pSrcWS = null;
//            try
//            {
//                if (flag == 0)
//                {
//                    OpenFileDialog ofd = new OpenFileDialog();
//                    ofd.Filter = "PGDB文件(.mdb)|*.mdb";
//                    ofd.Multiselect = false;
//                    if (ofd.ShowDialog() == DialogResult.OK)
//                    {
//                        string sFileName = ofd.FileName;
//                        pSrcWS = LSGISHelper.WorkspaceHelper.GetAccessWorkspace(sFileName);
//                    }
//                }
//                else
//                {
//                    FolderBrowserDialog fdb = new FolderBrowserDialog();
//                    if (fdb.ShowDialog() == DialogResult.OK)
//                    {
//                        string sFileName = fdb.SelectedPath;
//                        pSrcWS = LSGISHelper.WorkspaceHelper.GetFGDBWorkspace(sFileName);
//                    }
//                }
//                if (pSrcWS != null)
//                {
//                    IEnumDatasetName pEnumDSName = pSrcWS.get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
//                    IDatasetName pDName = pEnumDSName.Next();

//                    while (pDName != null)
//                    {
//                        clsExportClass.ConvertFeatureDataset(pSrcWS, pDesWS, pDName.Name, pDName.Name);

//                        pDName = pEnumDSName.Next();
//                    }
//                    LSCommonHelper.MessageBoxHelper.ShowMessageBox("导入成功");
//                }
//            }
//            catch { }
//        }

//        public static void ImportShapefile2SDE(IWorkspace pDesWS, TaskMonitor mTaskMonitor,
//            IFeatureDatasetName pFDN)
//        {
//            OpenFileDialog ofd = new OpenFileDialog();
//            ofd.Title = "打开SHP数据";
//            ofd.Filter = "SHP数据(*.shp)|*.shp";
//            ofd.Multiselect = true;
//            ofd.RestoreDirectory = true;
//            if (ofd.ShowDialog() == DialogResult.OK)
//            {
//                string[] sFileNames = ofd.FileNames;
//                string sFileName = "";
//                IFeatureClass pFC = null;

//                mTaskMonitor.EnterWaitState();
//                string sName = "";
//                IWorkspace pSrcWS = null;
//                for (int i = 0; i < sFileNames.Length; i++)
//                {
//                    mTaskMonitor.TaskCaption = "共" + sFileNames.Length + "个文件，先处理第" + (i + 1) + "个文件";
//                    mTaskMonitor.TaskProgress = LSCommonHelper.MathHelper.Precent(0, sFileNames.Length, i);
//                    sFileName = sFileNames[i].ToString();
//                    pSrcWS = LSGISHelper.WorkspaceHelper.GetShapefileWorkspace(sFileName);
//                    sFileName = System.IO.Path.GetFileNameWithoutExtension(sFileName);
//                    IFeatureWorkspace pFWS = pSrcWS as IFeatureWorkspace;
//                    pFC = pFWS.OpenFeatureClass(sFileName);
//                    sName = (pFC as IDataset).Name;
//                    if (ConvertFeatureClass2FeatureDataset(pSrcWS, pDesWS, sName, sName, pFDN))
//                    { }
//                }
//                mTaskMonitor.ExitWaitState();
//                LSCommonHelper.MessageBoxHelper.ShowMessageBox("导入成功");
//            }
//        }
//        public static bool ConvertFeatureClass2FeatureDataset(IWorkspace sourceWorkspace,
//      IWorkspace targetWorkspace, string nameOfSourceFeatureClass,
//      string nameOfTargetFeatureClass, IFeatureDatasetName pName)
//        {
//            try
//            {
//                //create source workspace name   
//                IDataset sourceWorkspaceDataset = (IDataset)sourceWorkspace;
//                IWorkspaceName sourceWorkspaceName = (IWorkspaceName)sourceWorkspaceDataset.FullName;
//                //create source dataset name     
//                IFeatureClassName sourceFeatureClassName = new FeatureClassNameClass();
//                IDatasetName sourceDatasetName = (IDatasetName)sourceFeatureClassName;
//                sourceDatasetName.WorkspaceName = sourceWorkspaceName;
//                sourceDatasetName.Name = nameOfSourceFeatureClass;

//                //create target workspace name     
//                IDataset targetWorkspaceDataset = (IDataset)targetWorkspace;
//                IWorkspaceName targetWorkspaceName = (IWorkspaceName)targetWorkspaceDataset.FullName;
//                //create target dataset name      
//                IFeatureClassName targetFeatureClassName = new FeatureClassNameClass();
//                IDatasetName targetDatasetName = (IDatasetName)targetFeatureClassName;
//                targetDatasetName.WorkspaceName = targetWorkspaceName;
//                targetDatasetName.Name = nameOfTargetFeatureClass;
//                //Open input Featureclass to get field definitions.    
//                ESRI.ArcGIS.esriSystem.IName sourceName = (ESRI.ArcGIS.esriSystem.IName)sourceFeatureClassName;
//                IFeatureClass sourceFeatureClass = (IFeatureClass)sourceName.Open();
//                //Validate the field names because you are converting between different workspace types.     
//                IFieldChecker fieldChecker = new FieldCheckerClass();
//                IFields targetFeatureClassFields;
//                IFields sourceFeatureClassFields = sourceFeatureClass.Fields;
//                IEnumFieldError enumFieldError;
//                // Most importantly set the input and validate workspaces!   
//                fieldChecker.InputWorkspace = sourceWorkspace;
//                fieldChecker.ValidateWorkspace = targetWorkspace;
//                fieldChecker.Validate(sourceFeatureClassFields, out enumFieldError,
//                    out targetFeatureClassFields);
//                // Loop through the output fields to find the geomerty field     
//                IField geometryField;
//                for (int i = 0; i < targetFeatureClassFields.FieldCount; i++)
//                {
//                    if (targetFeatureClassFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
//                    {
//                        geometryField = targetFeatureClassFields.get_Field(i);
//                        // Get the geometry field's geometry defenition            
//                        IGeometryDef geometryDef = geometryField.GeometryDef;
//                        //Give the geometry definition a spatial index grid count and grid size       
//                        IGeometryDefEdit targetFCGeoDefEdit = (IGeometryDefEdit)geometryDef;
//                        targetFCGeoDefEdit.GridCount_2 = 1;
//                        targetFCGeoDefEdit.set_GridSize(0, 0);
//                        //Allow ArcGIS to determine a valid grid size for the data loaded       
//                        targetFCGeoDefEdit.SpatialReference_2 = geometryField.GeometryDef.SpatialReference;
//                        // we want to convert all of the features      
//                        IQueryFilter queryFilter = new QueryFilterClass();
//                        queryFilter.WhereClause = "";
//                        // Load the feature class              
//                        IFeatureDataConverter fctofc = new FeatureDataConverterClass();
//                        IEnumInvalidObject enumErrors = fctofc.ConvertFeatureClass(sourceFeatureClassName,
//                            queryFilter, pName, targetFeatureClassName,
//                            geometryDef, targetFeatureClassFields, "", 1000, 0);
//                        break;
//                    }
//                }
//                return true;
//            }
//            catch (Exception ex) { return false; }
//        }
//    }

//}
