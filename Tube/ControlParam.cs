using UnityEngine;
using System.Collections;
using ffDevelopmentSpace;
using UnityEngine.UI;


/* 
    Author:     fyw 
    CreateDate: 2018-02-07 18:03:01 
    Desc:       注释 
*/


public class ControlParam : MonoBehaviour 
{
    public Text showTxt;
    private int param = 0;

	#region public property
    #endregion
	#region private property
    #endregion

    #region unity function
    void OnEnable()
    {
    }
    void Start () 
	{
        Updateparam();
    }   
	void Update () 
	{
	}
    void OnDisable()
    {
    }
    void OnDestroy()
    {
    }
    #endregion

	#region public function
    public void AddParam()
    {
        param++;
        Updateparam();
    }
    public void ReduceParam()
    {
        param--;
        Updateparam();
    }
    public void Updateparam()
    {
        if (showTxt) showTxt.text = "" + param;
    }
	#endregion
	#region private function
	#endregion

    #region event function
    #endregion
}
