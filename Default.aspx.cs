using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string [] a = {"1","2","3" };
        string [] b = {"you", "me", "he" };
        string [] c = {"test1", "test2", "test3" };

        DataTable dt = new DataTable();
        //設定DataTable的欄位名稱和資料型別
        dt.Columns.Add("學號", Type.GetType("System.String"));
        dt.Columns.Add("英文", Type.GetType("System.String"));
        dt.Columns.Add("數學", Type.GetType("System.String"));
        //利用回圈存入陣列
        for (int i = 0; i < a.Length; i++)
        {
            DataRow dr = dt.NewRow();
            dr["學號"] = a[i];
            dr["英文"] = b[i];
            dr["數學"] = c[i];
            dt.Rows.Add(dr);
        }
        //將DataTable資料顯示於GridView
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
}
