using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class Hole : Image
{
    public override Material materialForRendering
    {
        get
        {
            Material baseMat = base.materialForRendering;
            Material holeMat = new Material(baseMat);
            holeMat.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return holeMat;
        }
    }
}
