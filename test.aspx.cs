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
	/// Summary description for WebForm4.
	/// </summary>
	public class WebForm4 : System.Web.UI.Page
	{
		private QuizSupp.Support m_oSupp;
		private void Page_Load(object sender, System.EventArgs e)
		{
			m_oSupp = new QuizSupp.Support((Page)this, Server.MapPath("."));
			// Put user code to initialize the page here
		}
		public string SCount()
		{
			m_oSupp.BumpNamedCounter("QuizGen2");
			return m_oSupp.GetNamedCounter("QuizGen2").ToString();
		}

		public void SBumpCount()
		{
			m_oSupp.BumpNamedCounter("QuizGen");
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
