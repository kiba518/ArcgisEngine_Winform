using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArcGISFormsApp1
{
    public partial class FormTable : Form
    {
        IFeatureLayer pFeatureLayer;
        public FormTable(IFeatureLayer _pFeatureLayer)
        {
            pFeatureLayer = _pFeatureLayer;
            InitializeComponent();
            Itable2Dtable();
        }
        public void Itable2Dtable()
        {

            IFields pFields;
            pFields = pFeatureLayer.FeatureClass.Fields;
            dtGridView.ColumnCount = pFields.FieldCount;
            for (int i = 0; i < pFields.FieldCount; i++)
            {

                string fldName = pFields.get_Field(i).Name;
                dtGridView.Columns[i].Name = fldName;
                dtGridView.Columns[i].ValueType = System.Type.GetType(ParseFieldType(pFields.get_Field(i).Type));
            }
            IFeatureCursor pFeatureCursor;

            pFeatureCursor = pFeatureLayer.FeatureClass.Search(null, false);

            IFeature pFeature;
            pFeature = pFeatureCursor.NextFeature();
            while (pFeature != null)
            {
                string[] fldValue = new string[pFields.FieldCount];
                for (int i = 0; i < pFields.FieldCount; i++)
                {
                    string fldName;
                    fldName = pFields.get_Field(i).Name;
                    if (fldName == pFeatureLayer.FeatureClass.ShapeFieldName)
                    {
                        fldValue[i] = Convert.ToString(pFeature.Shape.GeometryType);
                    }
                    else
                        fldValue[i] = Convert.ToString(pFeature.get_Value(i));
                }
                dtGridView.Rows.Add(fldValue);
                pFeature = pFeatureCursor.NextFeature();

            }

        }
        /// <summary>
        /// 获取esri数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ParseFieldType(esriFieldType type)
        {
            string esritype = type.ToString();
            //对应格式的字符串转化
            switch (esritype)
            {
                case "esriFieldTypeInteger":
                    return typeof(int).ToString();
                case "esriFieldTypeString":
                    return typeof(string).ToString();
                case "esriFieldTypeDouble":
                    return typeof(double).ToString();
                case "esriFieldTypeDate":
                    return typeof(DateTime).ToString();
                default:
                    return typeof(string).ToString();
            }
        }

        /// <summary>
        /// 获取esri数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static esriFieldType GetEsriType(string type)
        {
            switch (type)//匹配类型选择
            {
                case "System.String":
                    return esriFieldType.esriFieldTypeString;
                case "System.DateTime":
                    return esriFieldType.esriFieldTypeDate;
                case "System.Double":
                    return esriFieldType.esriFieldTypeDouble;
                case "System.Int32":
                case "System.Int16":
                case "System.Int64":
                    return esriFieldType.esriFieldTypeInteger;
                case "System.Date":
                    return esriFieldType.esriFieldTypeDate;
                default:
                    return esriFieldType.esriFieldTypeString;
            }
        }

        /// <summary>
        /// 获取esri对应的datatable数据类型
        /// </summary>
        /// <param name="esritype"></param>
        /// <returns></returns>
        public static Type GetDataType(string esritype)
        {
            //对应格式的字符串转化
            switch (esritype)
            {
                case "esriFieldTypeInteger":
                    return typeof(int);
                case "esriFieldTypeString":
                    return typeof(string);
                case "esriFieldTypeDouble":
                    return typeof(double); 
                case "esriFieldTypeDate":
                    return typeof(DateTime);
                default:
                    return typeof(string);
            }
        }


    }
}
