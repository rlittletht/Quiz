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

namespace quiz
{
	/// <summary>
	/// Summary description for WebForm3.
	/// </summary>
	public class WebForm3 : System.Web.UI.Page
	{
		public QuizSupp.Support oSupp;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			oSupp = new QuizSupp.Support((Page)this, Server.MapPath("."));
			oSupp.GenerateGradedQuiz(Request.Form);
		}

		string SAppend(string s, string sAppend)
		{
			if (s.Length > 0)
				return s + "&" + sAppend;

			return s + "?" + sAppend;
		}

        public string SGetQuizParms()
        {
            string s = "";

			if (Request.Form.GetValues("quizParms") != null && Request.Form.GetValues("quizParms")[0].Length > 0)
				return Request.Form.GetValues("quizParms")[0];

			return "";
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
	}
}
