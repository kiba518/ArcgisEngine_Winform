using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcGISFormsApp1
{
    //绘制点 线 面枚举
    public enum Operation
    {
        Point,//绘制点
        PolyLine,//绘制线
        Polygon,//绘制面
        PolygonSelectAndFlash,//绘制面
        Nothing
    }
}
