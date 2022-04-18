using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;

namespace ArcGISFormsApp1
{
    class Class1
    {
        //数据转换主要涉及复制和转换数据出入 Geodatabases 的对象。 两个主要的数据转换对象是 FeatureDataConverter 和 GeoDBDataTransfer
        IWorkspaceFactory pSwf = new AccessWorkspaceFactoryClass();
        IWorkspaceFactory pDwf = new AccessWorkspaceFactoryClass();
        //调用例子 ConvertFeatureClass(pSwf, "E:\\s.mdb", "s", pDwf, "E:\\d.mdb", "d");
        public void ConvertFeatureClass(IWorkspaceFactory _pSWorkspaceFactory, String _pSWs, string _pSName, IWorkspaceFactory _pTWorkspaceFactory, String _pTWs, string _pTName)
        {
            // Open the source and target workspaces.
            IWorkspace pSWorkspace = _pSWorkspaceFactory.OpenFromFile(_pSWs, 0);
            IWorkspace pTWorkspace = _pTWorkspaceFactory.OpenFromFile(_pTWs, 0);

            IFeatureWorkspace pFtWs = pSWorkspace as IFeatureWorkspace;
            IFeatureClass pSourceFeatureClass = pFtWs.OpenFeatureClass(_pSName);
            IDataset pSDataset = pSourceFeatureClass as IDataset;

            IFeatureClassName pSourceFeatureClassName = pSDataset.FullName as IFeatureClassName;

            IDataset pTDataset = (IDataset)pTWorkspace;
            IName pTDatasetName = pTDataset.FullName;

            IWorkspaceName pTargetWorkspaceName = (IWorkspaceName)pTDatasetName;

            IFeatureClassName pTargetFeatureClassName = new FeatureClassNameClass();

            IDatasetName pTargetDatasetName = (IDatasetName)pTargetFeatureClassName;
            pTargetDatasetName.Name = _pTName;

            pTargetDatasetName.WorkspaceName = pTargetWorkspaceName;

            // 创建字段检查对象

            IFieldChecker pFieldChecker = new FieldCheckerClass();

            IFields sourceFields = pSourceFeatureClass.Fields;
            IFields pTargetFields = null;

            IEnumFieldError pEnumFieldError = null;

            pFieldChecker.InputWorkspace = pSWorkspace;
            pFieldChecker.ValidateWorkspace = pTWorkspace;

            // 验证字段

            pFieldChecker.Validate(sourceFields, out pEnumFieldError, out pTargetFields);
            if (pEnumFieldError != null)
            {
                // Handle the errors in a way appropriate to your application. Console.WriteLine("Errors were encountered during field validation.");
            }

            String pShapeFieldName = pSourceFeatureClass.ShapeFieldName;

            int pFieldIndex = pSourceFeatureClass.FindField(pShapeFieldName);
            IField pShapeField = sourceFields.get_Field(pFieldIndex);

            IGeometryDef pTargetGeometryDef = pShapeField.GeometryDef;

            // 创建要素转换对象

            IFeatureDataConverter pFDConverter = new FeatureDataConverterClass();

            IEnumInvalidObject pEnumInvalidObject = pFDConverter.ConvertFeatureClass(pSourceFeatureClassName, null, null, pTargetFeatureClassName, pTargetGeometryDef, pTargetFields, "", 1000, 0);

            // Check for errors.

            IInvalidObjectInfo pInvalidInfo = null;
            pEnumInvalidObject.Reset();

            while ((pInvalidInfo = pEnumInvalidObject.Next()) != null)
            {
                // Handle the errors in a way appropriate to the application.
                Console.WriteLine("Errors occurred for the following feature: {0}",
                  pInvalidInfo.InvalidObjectID);
            }
        }


        /// <summary>
        /// 在 ArcGIS Engine 中要使用查询图层，我们要了解一个接口 ISqlWorkspace，从这个接口的名称也容 易看出，这个接口可以和 SQL 打交道，这正是 QueryLayer 的一个特点。
        /// 在帮助文件中我们可以获得 ISqlWorkspace 的详细信息，ISqlWorkspace.GetTables（）返回 IStringArray 类型的变量，用这个方法我们可以获取数据库中所 有的表的名称。
        /// ISqlWorkspace.OpenQueryCursor（）这个方法通过传入一个过滤语句，返回一个游标； ISqlWorkspace.OpenQueryClass（）返回通过过滤条件返回 ITable 类型的对象。
        ///  ISqlWorkspace 被 SqlWorkspaceClass 实现，而 SqlWorkspaceClass 同时实现了 IWorkspace 接口。那 也就意味着 ISqlWorkspace 的使用和 IWorksapce 的使用是类似的，
        ///  我们可以按照以下步骤来执行一个 QueryLayer。
        /// </summary>
        /// <returns></returns>
        public IFeatureLayer OracleQueryLayer()
        {

            // 创建SqlWorkspaceFactory的对象
            Type pFactoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.SqlWorkspaceFactory");
            IWorkspaceFactory pWorkspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(pFactoryType);

            // 构造连接数据库的参数
            IPropertySet pConnectionProps = new PropertySetClass();
            pConnectionProps.SetProperty("dbclient", "Oracle11g");
            pConnectionProps.SetProperty("serverinstance", "esri");
            pConnectionProps.SetProperty("authentication_mode", "DBMS");

            pConnectionProps.SetProperty("user", "scott");
            pConnectionProps.SetProperty("password", "arcgis");

            // 打开工作空间
            IWorkspace workspace = pWorkspaceFactory.Open(pConnectionProps, 0);
            ISqlWorkspace pSQLWorkspace = workspace as ISqlWorkspace;

            //获取数据库中的所有表的名称
            IStringArray pStringArray = pSQLWorkspace.GetTables();
            for (int i = 0; i < pStringArray.Count; i++)
            {
                Console.WriteLine(pStringArray.get_Element(i));
            }

            // 构造过滤条件 SELECT \* FROM PointQueryLayer

            IQueryDescription queryDescription = pSQLWorkspace.GetQueryDescription("SELECT * FROM PointQueryLayer");
            ITable pTable = pSQLWorkspace.OpenQueryClass("QueryLayerTest", queryDescription);

            IFeatureLayer pFeatureLayer = new FeatureLayerClass();
            pFeatureLayer.FeatureClass = pTable as IFeatureClass;
            return pFeatureLayer;
        }


        //叠加分析  交集（Intersect）
        public IFeatureClass Intsect(IFeatureClass _pFtClass, IFeatureClass _pFtOverlay, string _FilePath, string _pFileName)

        {

            IFeatureClassName pOutPut = new FeatureClassNameClass();
            pOutPut.ShapeType = _pFtClass.ShapeType;
            pOutPut.ShapeFieldName = _pFtClass.ShapeFieldName;
            pOutPut.FeatureType = esriFeatureType.esriFTSimple;

            //set output location and feature class name 
            IWorkspaceName pWsN = new WorkspaceNameClass();

            pWsN.WorkspaceFactoryProgID = "esriDataSourcesFile.ShapefileWorkspaceFactory";
            pWsN.PathName = _FilePath;

            //也可以用这种方法,IName 和IDataset的用法

            /* 
            IWorkspaceFactory pWsFc = new ShapefileWorkspaceFactoryClass ();
            IWorkspace pWs = pWsFc.OpenFromFile (_FilePath, 0);

            IDataset pDataset = pWs as IDataset;

            IWorkspaceName pWsN = pDataset.FullName as IWorkspaceName;
            */

            IDatasetName pDatasetName = pOutPut as IDatasetName;

            pDatasetName.Name = _pFileName;
            pDatasetName.WorkspaceName = pWsN;

            IBasicGeoprocessor pBasicGeo = new BasicGeoprocessorClass();

            IFeatureClass pFeatureClass = pBasicGeo.Intersect(_pFtClass as ITable, false, _pFtOverlay as ITable, false, 0.1, pOutPut);

            return pFeatureClass;

        }
        //叠加分析 、裁减（Clip）、合并叠加（Union）以及合并（Merge）
    }
}
