using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

namespace quiz
{
	/// <summary>
	/// Summary description for WebForm2.
	/// </summary>
	public class WebForm2 : System.Web.UI.Page
	{
		public QuizSupp.Support oSupp;
		//protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;
	//	protected System.Web.UI.WebControls.RadioButtonList RadioButtonList1;
//		protected System.Web.UI.WebControls.RadioButton RadioButton1;
//		protected System.Web.UI.WebControls.RadioButton RadioButton2;
//		protected System.Web.UI.HtmlControls.HtmlTextArea TEXTAREA1;
//		protected System.Web.UI.HtmlControls.HtmlTextArea TEXTAREA1;
		
		protected System.Web.UI.WebControls.Button Button1;

		private string GetOnOffFormVal(string sControl)
		{
		    return Request.Form.GetValues(sControl) != null ? Request.Form.GetValues(sControl)[0] : "off";
		}

		private int GetFormIntVal(string sControl, int nDefault)
		{
			return Request.Form.GetValues(sControl) != null ? Int32.Parse(Request.Form.GetValues(sControl)[0]) : nDefault;
		}

		private int? GetFormNullableIntVal(string sControl)
		{
			try
				{
				return Request.Form.GetValues(sControl) != null ? Int32.Parse(Request.Form.GetValues(sControl)[0]) : (int?) null;
				}
			catch
				{
				return null;
				}
		}

		private bool? GetFormNullableBoolVal(string sControl)
		{
			try
				{
				return Request.Form.GetValues(sControl) != null && Request.Form.GetValues(sControl)[0].Length > 0 ? (Request.Form.GetValues(sControl)[0] == "false" ? false : true) : (bool?) null;
				}
			catch
				{
				return null;
				}
		}

		public string SGetQuizParms()
		{
			return Request.Params["quizParms"] ?? "";
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			oSupp = new QuizSupp.Support((Page)this, Server.MapPath("."));

			string sMinors = GetOnOffFormVal("MINORS");
            string sMajors = GetOnOffFormVal("MAJORS");
            string sJrSrBl = GetOnOffFormVal("JRSRBL");
            string sFed = GetOnOffFormVal("FED");
            string sSoftball = GetOnOffFormVal("SOFTBALL");
            string sLocal = GetOnOffFormVal("LOCAL");
			string sDebugList = Request.Params["QDEBUG"] ?? null;
			string sDiff1 = GetOnOffFormVal("DIFF1");
            string sDiff2 = GetOnOffFormVal("DIFF2");
            string sDiff3 = GetOnOffFormVal("DIFF3");
            string sDiff4 = GetOnOffFormVal("DIFF4");
            int nCount = GetFormIntVal("COUNT", 5);

            oSupp.GenerateQuiz(nCount, sMinors, sMajors, sJrSrBl, sSoftball, sLocal, sFed, sDiff1, sDiff2, sDiff3, sDiff4, sDebugList);
			
			// Put user code to initialize the page here
			// increment the hit counter
			oSupp.BumpNamedCounter("QuizGen");
		}

	    #region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Button1_Click(object sender, System.EventArgs e) {
//			TEXTAREA1.Value = "foo";
			Control ctrl = this.FindControl("TEXTAREA0");
			
			if (ctrl != null)
				{
				((HtmlTextArea)ctrl).Value += "foobar";
				}
			 ctrl = this.FindControl("TEXTAREA1");
			
			if (ctrl != null)
				{
				((HtmlTextArea)ctrl).Value += "foo1bar";
				}
		}

		private void Submit1_ServerClick(object sender, System.EventArgs e) {
			this.Response.Write("222Testing...");
			foreach (string key in this.Session.Keys)
			{
			this.Response.Write(key);
			}
			this.Response.Write(String.Format("items: {0}", this.Session.Keys.Count));
//			TEXTAREA1.Value += "foo";
			//ButtonRadioButtonList1.Items[0].Value
		}

		
	}
}
