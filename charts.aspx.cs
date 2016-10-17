using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;

public partial class webs_charts : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public void Button1_Click(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();
        Response.Redirect("home.aspx");
    }

    [WebMethod]
    public static List<ChartDetails> GetDataonload()
    {

        /*
        資料庫變數宣告
        */
        string strConn = "Data Source=140.127.22.24,1433;Network Library = DBMSSOCN; Initial Catalog =Aquaculture;User ID = water1; Password = water;";
        SqlConnection sqlConn = new SqlConnection(strConn);
        String strSQL = @"SELECT feeding_record.batch_id, pool.poolname, feeding_record.feedingcount, food_supplier.feedname,feeding_record.feedingdate
FROM batch INNER JOIN
fish ON batch.fish_id = fish.id INNER JOIN
pool ON batch.currentpool_id = pool.id INNER JOIN
feeding_record ON batch.id = feeding_record.batch_id INNER JOIN
food_supplier ON feeding_record.foodsupplier_id = food_supplier.foodsupplierid
WHERE poolname='C3'";
        SqlDataReader dr = null;
        SqlCommand command = new SqlCommand(strSQL, sqlConn);
        /*
        欲傳送之資料陣列宣告
        */
        List<ChartDetails> dataList = new List<ChartDetails>();
        sqlConn.Open();
        dr = command.ExecuteReader();

        while (dr.Read())
        {
            /*測試變數 先以亂數給訂日期*/
            Random crandom = new Random(Guid.NewGuid().GetHashCode());
            int day = crandom.Next(29) + 1;


            ChartDetails details = new ChartDetails();
            details.date = new DateTime(int.Parse(dr["feedingdate"].ToString().Substring(0, 4)), int.Parse(dr["feedingdate"].ToString().Substring(4, 2)), int.Parse(dr["feedingdate"].ToString().Substring(6, 2))).ToString("yyyy-MM-ddTHH:mm:ss");
            details.type = "pin";
            details.backgroundColor = "#85CDE6";
            details.graph = "g1";
            details.description = dr["feedname"].ToString() + "投餵" + dr["feedingcount"].ToString() + "包";
            details.text = dr["feedingcount"].ToString();
            dataList.Add(details);
        }
        dr.Close();
        sqlConn.Close();


        /*
        資料庫變數宣告
        */
        string strConn2 = "Data Source=140.127.22.24,1433;Network Library = DBMSSOCN; Initial Catalog =Aquaculture;User ID = water1; Password = water;";
        SqlConnection sqlConn2 = new SqlConnection(strConn2);
        String strSQL2 = @"SELECT batch.currentpool_id, pool.poolname, measuring.totalweight, measuring.loss,measuring.measurementdate,measuring.mantissa
                          FROM batch INNER JOIN
                          pool ON batch.currentpool_id = pool.id INNER JOIN
                          measuring ON batch.id = measuring.batch_id
                          WHERE poolname='C3'";
        SqlDataReader dr2 = null;
        SqlCommand command2 = new SqlCommand(strSQL2, sqlConn2);
        sqlConn2.Open();
        dr2 = command2.ExecuteReader();
        while (dr2.Read())
        {


            ChartDetails details2 = new ChartDetails();
            details2.date = new DateTime(int.Parse(dr2[4].ToString().Substring(0, 4)), int.Parse(dr2[4].ToString().Substring(4, 2)), int.Parse(dr2[4].ToString().Substring(6, 2))).ToString("yyyy-MM-ddTHH:mm:ss");
            details2.type = "pin";
            details2.backgroundColor = "#FF3D00";
            details2.graph = "g1";
            details2.description = "耗損" + dr2["loss"].ToString() + "隻";
            details2.text = dr2["loss"].ToString();
            dataList.Add(details2);
        }
        dr2.Close();
        sqlConn.Close();





        return dataList;

    }

    //此方法用於呼叫出所有測量魚重紀錄
    [WebMethod]
    public static List<ChartValues> GetChartValues()
    {

        /*
        資料庫變數宣告
        */
        string strConn = "Data Source=140.127.22.24,1433;Network Library = DBMSSOCN; Initial Catalog =Aquaculture;User ID = water1; Password = water;";
        SqlConnection sqlConn = new SqlConnection(strConn);
        String strSQL = @"SELECT batch.currentpool_id, pool.poolname, measuring.totalweight, measuring.loss,measuring.measurementdate,measuring.mantissa
                          FROM batch INNER JOIN
                          pool ON batch.currentpool_id = pool.id INNER JOIN
                          measuring ON batch.id = measuring.batch_id
                          WHERE poolname='C3'";
        SqlDataReader dr = null;
        SqlCommand command = new SqlCommand(strSQL, sqlConn);
        /*
        欲傳送之資料陣列宣告
        */
        List<ChartValues> dataList = new List<ChartValues>();
        sqlConn.Open();
        dr = command.ExecuteReader();
        /*
        控制變數宣告
        */
        DateTime dt_tempdate = new DateTime(2013, 11, 16);
        float flo_tempvalue = 0;



        while (dr.Read())
        {


            //補足沒資料的天數
           
            while (dt_tempdate <= new DateTime(int.Parse(dr["measurementdate"].ToString().Substring(0, 4)), int.Parse(dr["measurementdate"].ToString().Substring(4, 2)), int.Parse(dr["measurementdate"].ToString().Substring(6, 2))))
            {
                ChartValues mdetails = new ChartValues();
                mdetails.date = dt_tempdate.ToString("yyyy-MM-ddTHH:mm:ss");
                mdetails.value = flo_tempvalue;
                dataList.Add(mdetails);
                dt_tempdate = dt_tempdate.AddHours(24);
            }
            ChartValues details = new ChartValues();
            details.date = new DateTime(int.Parse(dr["measurementdate"].ToString().Substring(0, 4)), int.Parse(dr["measurementdate"].ToString().Substring(4, 2)), int.Parse(dr["measurementdate"].ToString().Substring(6, 2))).ToString("yyyy-MM-ddTHH:mm:ss");
            details.value = float.Parse(dr["totalweight"].ToString())/float.Parse(dr["mantissa"].ToString());
            dataList.Add(details);
            dt_tempdate = new DateTime(int.Parse(dr["measurementdate"].ToString().Substring(0, 4)), int.Parse(dr["measurementdate"].ToString().Substring(4, 2)), int.Parse(dr["measurementdate"].ToString().Substring(6, 2)));
            flo_tempvalue =float.Parse(dr["totalweight"].ToString())/float.Parse(dr["mantissa"].ToString());



        }
        sqlConn.Close();
        return dataList;
    }

    public class ChartDetails
    {
        public string date { get; set; }
        public string type { get; set; }
        public string backgroundColor { get; set; }
        public string graph { get; set; }
        public string description { get; set; }
        public string text { get; set; }
        /*
                    date: newDate,
                    type: "sign",
                    backgroundColor: "#85CDE6",
                    graph: "g1",
                    text: "分",
                    description: "分魚 200 條"
        */
    }
    public class ChartValues
    {
        public string date { get; set; }
        public float value { get; set; }
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton imgbtn = (ImageButton)sender;
        Response.Redirect("charts" + imgbtn.TabIndex + ".aspx");
    }
}
