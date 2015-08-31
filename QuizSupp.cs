using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Configuration;


namespace QuizSupp
{
	public class Support
	{
		public string m_sSitTpl;
		public string sPathRoot;
		Page m_page;

	    private string m_sCounterConnString;
	    private string m_sQuestionsConnString;

//		string m_sCounterConnString = "Provider=SQLOLEDB; Data Source=db009.yeg01.ca.as4250.net;User ID=dba0902;Password=********;Initial Catalog=db0902";
//      string m_sQuestionsConnString = "Provider=SQLOLEDB; Data Source=db009.yeg01.ca.as4250.net;User ID=dba0902;Password=********;Initial Catalog=db0902";
//      string m_sCounterConnString = "Provider=SQLOLEDB; Data Source=cacofonix;Initial Catalog=db0902;Trusted_Connection=Yes";
//      string m_sQuestionsConnString = "Provider=SQLOLEDB; Data Source=cacofonix;Database=db0902;Trusted_Connection=Yes";

		/* S U P P O R T */
		/*----------------------------------------------------------------------------
			%%Function: Support
			%%Qualified: QuizSupp.Support.Support
			%%Contact: rlittle

			
		----------------------------------------------------------------------------*/
		public Support(Page page, string sPath)
		{
			m_page = page;
			sPathRoot = sPath;
		    m_sCounterConnString = ConfigurationManager.AppSettings["Thetasoft.Azure.ConnectionString"];
            m_sQuestionsConnString = ConfigurationManager.AppSettings["Thetasoft.Azure.ConnectionString"];
		}
			
		/* G E T  N A M E D  C O U N T E R */
		/*----------------------------------------------------------------------------
			%%Function: GetNamedCounter
			%%Qualified: QuizSupp.Support.GetNamedCounter
			%%Contact: rlittle

		----------------------------------------------------------------------------*/
		public int GetNamedCounter(string sCounter)
		{
			m_page.Trace.Warn("command: ");
			int nCount = 0;
			try
				{
				OleDbConnection oConn = new OleDbConnection(m_sCounterConnString);
                    
				m_page.Trace.Write("Connect=" + oConn.ConnectionString + " (User=" +Environment.UserName);
				oConn.Open();

				OleDbCommand oCmd = oConn.CreateCommand();
				string sCmd = "select * from Counters where CounterName = '" + sCounter +"'";
				oCmd.CommandText = sCmd;
				OleDbDataReader oReader = oCmd.ExecuteReader();

				oReader.Read();
				nCount = (int)oReader["Count"];
				oReader.Close();
				oCmd = null;
				oConn.Close();
				}
			catch (Exception exc) { nCount = -1; };
			
			return nCount;
		}

		public void BumpNamedCounter(string sCounter)
		{
			int c = GetNamedCounter(sCounter);

			c++;			
			
			OleDbConnection oConn = new OleDbConnection(m_sCounterConnString);

			m_page.Trace.Write("Connect=" + oConn.ConnectionString);
			oConn.Open();

			OleDbCommand oCmd = oConn.CreateCommand();
			string sCmd;
			if (c == 0)
				{
				sCmd = "INSERT INTO Counters (CounterName, [Count]) VALUES ('" + sCounter + "', 0)";
				}
			else
				{
				sCmd = "UPDATE Counters SET [Count] = " + c.ToString() + " WHERE CounterName = '" + sCounter +"'";
				}
				
			m_page.Trace.Write("command: "+sCmd);
			oCmd.CommandText = sCmd;
			oCmd.ExecuteNonQuery();
			
			oCmd = null;
			oConn.Close();
		}
		
		public class Question
		{
			public bool R1, R2, R3, BR;
			public bool fMinors, fMajors, fJrSrBl, fSoftball, fLocal, fFed;
			public int nDiff;
			public string sLocal;
			public int cOuts, cBalls, cStrikes;
			public string sQuestion;
			public string []rgsAnswers;
			public int nCorrectAnswer;
			public int id;
			public int nGivenAnswer;
			public int iQuestionTarget;
			public string sRuling;

			/* Q U E S T I O N */
			/*----------------------------------------------------------------------------
				%%Function: Question
				%%Qualified: QuizSupp.Support:Question.Question
				%%Contact: rlittle

				
			----------------------------------------------------------------------------*/
			public Question()
			{
				rgsAnswers = new string[4];
				rgsAnswers[0] = null;
				rgsAnswers[1] = null;
				rgsAnswers[2] = null;
				rgsAnswers[3] = null;
				fMinors = fMajors = fJrSrBl = fSoftball = fLocal = fFed = false;
			}

		};

		public ArrayList plQuestions;

		public struct MapQuestion
		{
			public int iQuestion;
			public int idQuestion;
			public int iKey;

			/* M A P  Q U E S T I O N */
			/*----------------------------------------------------------------------------
				%%Function: MapQuestion
				%%Qualified: QuizSupp.Support:MapQuestion.MapQuestion
				%%Contact: rlittle

				
			----------------------------------------------------------------------------*/
			public MapQuestion(int i, int id, int iKeyIn)
			{
				iQuestion = i; idQuestion = id; iKey = iKeyIn;
			}

		};

		public class SortMQIndex : IComparer
		{
			int IComparer.Compare(object x, object y)
			{
				return ((MapQuestion)x).iQuestion - ((MapQuestion)y).iQuestion;
			}
		}

		public class SortMQId : IComparer
		{
			int IComparer.Compare(object x, object y)
			{
				return ((MapQuestion)x).idQuestion - ((MapQuestion)y).idQuestion;
			}
		}

		/* M Q  F R O M  S T R I N G */
		/*----------------------------------------------------------------------------
			%%Function: MqFromString
			%%Qualified: QuizSupp.Support.MqFromString
			%%Contact: rlittle

			
		----------------------------------------------------------------------------*/
		public MapQuestion MqFromString(string s, int iKey)
		{
			int i, id;
			int ich, ichBegin;

			ichBegin = 0;

			while (ichBegin < s.Length && !Char.IsDigit(s[ichBegin]))
				ichBegin++;

			if (ichBegin >= s.Length)
				return new MapQuestion(-1, -1, iKey);

			ich = s.IndexOf("_", ichBegin);

			if (ich >= 0)
				{
				i = Int32.Parse(s.Substring(ichBegin, ich - ichBegin));
				id = Int32.Parse(s.Substring(ich + 1));
				}
			else
				{
				i = Int32.Parse(s);
				id = 0;
				}
			return new MapQuestion(i, id, iKey);
		}

		/* Q  R E A D */
		/*----------------------------------------------------------------------------
			%%Function: QRead
			%%Qualified: QuizSupp.Support.QRead
			%%Contact: rlittle

			
		----------------------------------------------------------------------------*/
		public Question QRead(OleDbDataReader oReader, int nGivenAnswer, int iQuestion)
		{
			Question q = new Question();

			q.nGivenAnswer = nGivenAnswer;
			q.iQuestionTarget = iQuestion;

			q.sRuling = String.Copy((string)oReader["Ruling"]);
			q.R1 = (bool)(oReader["R1"]);
			q.R2 = (bool)oReader["R2"];
			q.R3 = (bool)oReader["R3"];
			q.BR = (bool)oReader["BR"];
			q.cOuts = (int)oReader["Outs"];
			q.cBalls = (int)oReader["Balls"];
			q.cStrikes = (int)oReader["Strikes"];
			q.sQuestion = String.Copy((string)oReader["Question"]);
			q.id = (int)oReader["ID"];
            q.fMinors = (bool)(oReader["Minors"]);
            q.fMajors = (bool)(oReader["Majors"]);
            q.fJrSrBl = (bool)(oReader["JrSrBl"]);
            q.fSoftball = (bool)(oReader["Softball"]);
            q.fLocal = (bool)(oReader["LocalRule"]);
            q.fFed = (bool)(oReader["NFHS"]);
            q.nDiff = (int)(oReader["Difficulty"]);
            
            if (q.fLocal)
                q.sLocal = (string)oReader["LocalLeague"];
                
			if (oReader["Answer1"] != DBNull.Value
				&& ((string)(oReader["Answer1"])).Length > 0)
				{
				q.rgsAnswers[0] = String.Copy((string)oReader["Answer1"]);
				}

			if (oReader["Answer2"] != DBNull.Value
				&& ((string)(oReader["Answer2"])).Length > 0)
				{
				q.rgsAnswers[1] = String.Copy((string)oReader["Answer2"]);
				}

			if (oReader["Answer3"] != DBNull.Value
				&& ((string)(oReader["Answer3"])).Length > 0)
				{
				q.rgsAnswers[2] = String.Copy((string)oReader["Answer3"]);
				}

			if (oReader["Answer4"] != DBNull.Value
				&& ((string)(oReader["Answer4"])).Length > 0)
				{
				q.rgsAnswers[3] = String.Copy((string)oReader["Answer4"]);
				}

			q.nCorrectAnswer = (int)oReader["CorrectAnswer"];

			return q;
		}

		/* S O R T  Q U E S T I O N S  B Y  M A P */
		/*----------------------------------------------------------------------------
			%%Function: SortQuestionsByMap
			%%Qualified: QuizSupp.Support.SortQuestionsByMap
			%%Contact: rlittle

		----------------------------------------------------------------------------*/
		public void SortQuestionsByMap(ref ArrayList plmq)
		{
			for (int iq = 0; iq < plmq.Count; iq++)
				{
				while (((Question)plQuestions[iq]).iQuestionTarget != iq)
					{
					// not in its correct spot.  swap it with its correct location
					Question q;
					int iTarget = ((Question)plQuestions[iq]).iQuestionTarget;

					q = (Question)plQuestions[iTarget];
					plQuestions[iTarget] = plQuestions[iq];
					plQuestions[iq] = q;
					}
				}
		}

		/* G E N E R A T E  G R A D E D  Q U I Z */
		/*----------------------------------------------------------------------------
			%%Function: GenerateGradedQuiz
			%%Qualified: QuizSupp.Support.GenerateGradedQuiz
			%%Contact: rlittle

			
		----------------------------------------------------------------------------*/
		public bool GenerateGradedQuiz(NameValueCollection nvc)
		{
			ArrayList plmq = new ArrayList(nvc.Count);
			
			for (int i = 0; i < nvc.Count; i++)
				{
				if (nvc.GetKey(i)[0] != 'Q')
					continue;
				plmq.Add(MqFromString(nvc.GetKey(i), i));
				}
			if (plmq.Count == 0)
				return true;
				
			plmq.Sort(new SortMQId());

			// ok, pls is our sorted list of questions

			TextReader oTxtReader = new StreamReader(Path.Combine(sPathRoot, "bbdiamond.htm"));
			
			m_sSitTpl = oTxtReader.ReadToEnd();
			oTxtReader.Close();

			plQuestions = new ArrayList(plmq.Count);

            OleDbConnection oConn = new OleDbConnection(m_sQuestionsConnString);

			oConn.Open();

			OleDbCommand oCmd = oConn.CreateCommand();
			string sCmd = "select * from quiz_Questions where id in (";
			bool fFirst = true;

			foreach (MapQuestion mq in plmq)
				{
				if (!fFirst)
					sCmd += ",";

				sCmd += mq.idQuestion.ToString();
				fFirst = false;
				}

			sCmd += ") order by id";
			oCmd.CommandText = sCmd;

			OleDbDataReader oReader = oCmd.ExecuteReader();

			int iQuestion = 0;

			while (oReader.Read())
				{
				Question q = QRead(oReader, 
								   Int32.Parse(nvc.GetValues(((MapQuestion)plmq[iQuestion]).iKey)[0]),
								   ((MapQuestion)plmq[iQuestion]).iQuestion);

				plQuestions.Add(q);
				iQuestion++;
				}

			SortQuestionsByMap(ref plmq);
			return true;

		}

        public bool FFromString(string s)
        {
            if (String.Compare(s, "on", true) == 0 || String.Compare(s, "yes", true) == 0)
                return true;
            return false;
        }
         
        string SBuildWhere(bool fMinors, bool fMajors, bool fJrSrBl, bool fSoftball, string sLocal, bool fFed, bool fDiff1, bool fDiff2, bool fDiff3, bool fDiff4)
        {
            string s = " WHERE (";
            bool fFirst = true;
            
            if (fMinors)
                {
                if (!fFirst)
                    s += " OR ";
                s += " Minors=1 ";
                fFirst = false;
                }
            if (fMajors)
                {
                if (!fFirst)
                    s += " OR ";
                s += " Majors=1 ";
                fFirst = false;
                }
            if (fJrSrBl)
                {
                if (!fFirst)
                    s += " OR ";
                s += " JrSrBl=1 ";
                fFirst = false;
                }
            if (fSoftball)
                {
                if (!fFirst)
                    s += " OR ";
                s += " Softball=1 ";
                fFirst = false;
                }
            if (fFed)
                {
                if (!fFirst)
                    s += " OR ";
                s += " NFHS=1 ";
                fFirst = false;
                }
            s += ")";
            if (sLocal != null)
                {
                if (!fFirst)
                    s += " AND ";
                    
                s += " (LocalRule = 0 Or (LocalRule=1 AND LocalLeague = '" + sLocal + "'))";
                }
            else
                {
                if (!fFirst)
                    s += " AND ";

                s += " (LocalRule = 0)";
                }

            if (!fFirst)
                s += " AND ";
                
            bool fFirstAnd = true;
            s+= "(";
            if (fDiff1 || (!fDiff2 && !fDiff3 && !fDiff4))
                {
                if (!fFirstAnd)
                    s += " OR ";
                s += " Difficulty=1";
                fFirstAnd = false;
                }
            if (fDiff2)
                {
                if (!fFirstAnd)
                    s += " OR ";
                s += " Difficulty=2";
                fFirstAnd = false;
                }
            if (fDiff3)
                {
                if (!fFirstAnd)
                    s += " OR ";
                s += " Difficulty=3";
                fFirstAnd = false;
                }
            if (fDiff4)
                {
                if (!fFirstAnd)
                    s += " OR ";
                s += " Difficulty=4";
                fFirstAnd = false;
                }
            s += ")";
                            
            return s;
        }

		/* G E N E R A T E  Q U I Z */
		/*----------------------------------------------------------------------------
			%%Function: GenerateQuiz
			%%Qualified: QuizSupp.Support.GenerateQuiz
			%%Contact: rlittle

			
		----------------------------------------------------------------------------*/
		public bool GenerateQuiz(int cQuestions, string sMinors, string sMajors, string sJrSrBl, string sSoftball, string sLocal, string sFed, string sDiff1, string sDiff2, string sDiff3, string sDiff4, string sDebugList)
		{
		    bool fMinors = FFromString(sMinors);
            bool fMajors = FFromString(sMajors);
            bool fJrSrBl = FFromString(sJrSrBl);
            bool fSoftball = FFromString(sSoftball);
		    bool fFed = FFromString(sFed);
		    bool fDiff1 = FFromString(sDiff1);
            bool fDiff2 = FFromString(sDiff2);
            bool fDiff3 = FFromString(sDiff3);
            bool fDiff4 = FFromString(sDiff4);
		        
		    if (String.Compare("none", sLocal, true) == 0)
		        sLocal = null;
		        
			TextReader oTxtReader = new StreamReader(Path.Combine(sPathRoot, "bbdiamond.htm"));
			
			m_sSitTpl = oTxtReader.ReadToEnd();
			oTxtReader.Close();

			plQuestions = new ArrayList(cQuestions);

			OleDbConnection oConn = new OleDbConnection(m_sQuestionsConnString);

			oConn.Open();

			OleDbCommand oCmd = oConn.CreateCommand();
            string sWhere;
            
            if (sDebugList != null && sDebugList.Length > 0)
                {
                sWhere = " WHERE id in (" + sDebugList + ") ";
                }
            else
                {
                sWhere = SBuildWhere(fMinors, fMajors, fJrSrBl, fSoftball, sLocal, fFed, fDiff1, fDiff2, fDiff3, fDiff4);
                }


			oCmd.CommandText = "select count(*) from quiz_Questions " + sWhere;
            m_page.Trace.Write("QuestionQuery=" + oCmd.CommandText);
            int c = (int)oCmd.ExecuteScalar();

			if (cQuestions > c) 
				cQuestions = c;
				
			ArrayList plmq = new ArrayList();
			ArrayList pln = new ArrayList(c);

			for (int i = 0; i < c; i++)
				{
				pln.Add(i);
				}

			Random rnd = new Random();

			// now, choose cQuestions random items from plnIdentity
			for (int i = 0; i < cQuestions; i++)
				{
				int iRand = rnd.Next(0, (c - i - 1));

				plmq.Add(new MapQuestion(i, (int)pln[iRand], i));
				pln.RemoveAt(iRand);
				}

			plmq.Sort(new SortMQId());

            oCmd.CommandText = "select * from quiz_Questions " + sWhere;

			OleDbDataReader oReader = oCmd.ExecuteReader();

			int iQuestion = 0;
			int imq = 0;

			while (imq < plmq.Count && oReader.Read())
				{
				if (((MapQuestion)plmq[imq]).idQuestion == iQuestion)
					{
					// we can read this one
                    Question q = QRead(oReader, -1, ((MapQuestion)plmq[imq]).iQuestion);
					MapQuestion mq = (MapQuestion)plmq[imq];
					mq.idQuestion = q.id;	// set it to the real id
					plmq[imq] = mq;

					plQuestions.Add(q);
					imq++;
					}
				iQuestion++;
				}

			SortQuestionsByMap(ref plmq);
			return true;
		}

		private string SFormatCount(string s, int c)
		{
			string sOut;
			
			if (c == 0)
				sOut = String.Format("No {0}s", s);
			else if (c == 1)
				sOut = String.Format("1 {0}", s);
			else
				sOut = String.Format("{0} {1}s", c, s);
			
			return sOut;
		}			

        /* S  B U I L D  S P E C I A L */
        /*----------------------------------------------------------------------------
        	%%Function: SBuildSpecial
        	%%Qualified: QuizSupp.Support.SBuildSpecial
        	%%Contact: rlittle

        ----------------------------------------------------------------------------*/
        public string SBuildSpecial(Question q)
        {
			string s = "";
            
            // check for fed only
            if (q.fFed && !q.fJrSrBl && !q.fMajors && !q.fMinors)
                {
                s += "NFHS only. ";
                }
            else if (!q.fFed && (q.fJrSrBl && q.fMajors && q.fMinors))
				{
				s += "Little League only. ";
				}
			else
				{
				if (q.fJrSrBl)
					{
					// might be only for that
					if (!q.fMajors && !q.fMinors)
						{
						if (q.fFed)
							s += "90' Diamond only. ";
						else
							s += "Little League Junior/Senior/Big League only. ";
						}
					else
						{
						if (q.fMajors && q.fMinors == false)
							{
							// no pattern....
							if (q.fMinors)
								s += "Minors, Junior/Senior/Big League. "; // Huh?  what a wacky rule!
							else
								{
								if (q.fFed)
									s += "Little League Majors, Junior/Senior/Big League, NFHS only. ";	// huh?
								else
									s += "Little League Majors, Junior/Senior/Big League only. ";
								}
							}
						}
					}
				else
					{
					// ok, not for jr, sr, bl...
					if (!q.fMinors || !q.fMajors)
						{
						if (q.fMajors)
							{
							if (q.fFed)
								s += "NFHS, Little League Majors only. ";
							else
								s += "Little League Majors only. ";
							}
						else
							{
							if (q.fFed)
								s += "NFHS, Little League Minors only. ";
							else
								s += "Little League Minors only. ";
							}
						}
					else
						{
						if (q.fFed)
							s += "NFHS, Little League Minors/Majors only.";
						else
							s += "Little League Minors/Majors only.";
						}
					}
				}

			if (q.fSoftball && !(q.fMinors || q.fMajors || q.fJrSrBl))
				{
				if (q.fFed)
					s += "Softball only. ";
				else
					s += "Little League Softball only. ";
				}
			
			if (!q.fSoftball)
				s += "Baseball only. ";
			
			return s;
		}
        		
		/* S  G E N E R A T E  H T M L  F O R  Q U E S T I O N */
		/*----------------------------------------------------------------------------
			%%Function: SGenerateHtmlForQuestion
			%%Qualified: QuizSupp.Support.SGenerateHtmlForQuestion
			%%Contact: rlittle

			
		----------------------------------------------------------------------------*/
		public string SGenerateHtmlForQuestion(Question q, int iQuestion)
		{
			int nQuestion = iQuestion + 1;
			string sHTML = "<p><b>Question " + nQuestion.ToString() + "</b> [" + q.id.ToString() + "]:</p>";
			string sRunners = "";
			string sCount = "";

			sHTML += "<table border=0 cellpadding=2 cellspacing=0><tr>";
			// if (true || q.BR || q.R1 || q.R2 || q.R3 || q.cOuts > 0 || q.cBalls > 0 || q.cStrikes > 0)
				{

				// if any of them are set, then we have a situation diagram
                string sBR = "&nbsp;&nbsp;&nbsp;";
                string sR1 = "&nbsp;&nbsp;&nbsp;";
                string sR2 = "&nbsp;&nbsp;&nbsp;";
                string sR3 = "&nbsp;&nbsp;&nbsp;";
				string sBalls = "Balls:";
				string sStrikes = "Strikes:";
				string sOuts = "Outs:";
				string sBallsOn = "";
				string sBallsOff = "";
				string sStrikesOn = "";
				string sStrikesOff = "";
				string sOutsOn = "";
				string sOutsOff = "";

				if (q.BR) 	sBR = "BR";

				int cRunners = 0;

				if (q.R1)	
					{
					sR1 = "R1";
					cRunners++;
					}
				if (q.R2)	
					{
					sR2 = "R2";
					cRunners++;
					}
				if (q.R3)	
					{
					sR3 = "R3";
					cRunners++;
					}

				if (cRunners >= 3 && q.R1 && q.R2 && q.R3)
					sRunners += "  Bases are loaded.";
				else
					{
					if (cRunners == 1)
						sRunners += "  Runner on ";
					else if (cRunners > 1)
						sRunners += "  Runners on ";

					if (q.R1)
						sRunners += "1st";

					if (q.R2)
						{
						if (q.R1)
							sRunners += ", ";

						sRunners += "2nd";
						}

					if (q.R3)
						{
						if (q.R1 || q.R2)
							sRunners += ", ";

						sRunners += "3rd";
						}

					if (sRunners.Length > 0)
						sRunners += ".";
					}

				if (q.cBalls > 0 || q.cStrikes > 0 || q.cOuts > 0)
					{
					string sBallsOut, sStrikesOut, sOutsOut;

					sBallsOut = SFormatCount("ball", q.cBalls);
					sStrikesOut = SFormatCount("strike", q.cStrikes);
					sOutsOut = SFormatCount("out", q.cOuts);
						
					sCount = String.Format("  {0}, {1}, {2}.", sBallsOut, sStrikesOut, sOutsOut);

					for (int i = 0; i < 3; i++)
						{
						if (i < q.cBalls)
							sBallsOn += "l";
						else
							sBallsOff += "l";
	
						if (i < q.cStrikes)
							sStrikesOn += "l";
						else
							sStrikesOff += "l";
	
						if (i < q.cOuts)
							sOutsOn += "l";
						else
							sOutsOff += "l";
						}
					if (q.cBalls >= 4)
						sBallsOn += "l";
					else
						sBallsOff += "l";
					}
				else
					{
					sBalls = "";
					sStrikes = "";
					sOuts = "";
					}

				sHTML += "<td>"+String.Format(m_sSitTpl, sBR, sR1, sR2, sR3, sBalls, sBallsOn, sBallsOff, sStrikes, sStrikesOn, sStrikesOff, sOuts, sOutsOn, sOutsOff);
				}

			sHTML += "<td><p>";

            sHTML += SBuildSpecial(q);
            
            if (q.fLocal)
                {
                sHTML += "Local League: " + q.sLocal + ". ";
                }
                                
			if (sRunners.Length > 0 || sCount.Length > 0)
				{
				sHTML += sCount + sRunners + "&nbsp;&nbsp;";
				}

			sHTML += q.sQuestion + "";

			// show them the options, or give them the answer
			if (q.nGivenAnswer == -1)
				{
				sHTML += "<table border=0>";
				for (int i = 0; i < 4; i++)
					{
					if (q.rgsAnswers[i] != null)
						{
						sHTML += String.Format("<tr><td><INPUT type=radio name=Q{0}_{1} value={2}{3}><td>{4}", iQuestion, q.id, i + 1, "", (string)q.rgsAnswers[i]);
//						sHTML += String.Format("<tr><td><INPUT type=radio name=Q{0}_{1} value={2}{3}><td>{4}", iQuestion, q.id, i + 1, (q.nCorrectAnswer == (i + 1)) ? " CHECKED" : "", (string)q.rgsAnswers[i]);
						//sHTML += String.Format("<asp:ListItem Value='{0}'>{1}</asp:ListItem>", i, (string)q.rgsAnswers[i]);
						}
					}
				sHTML += String.Format("<tr><td><INPUT checkead=true type=radio name=Q{0}_{1} value={2}><td>{3}", iQuestion, q.id, 0, "No answer");
				sHTML += "</table>";
				}
			else
				{
				sHTML += "<p><b>Your answer:</b> ";
				if (q.nGivenAnswer == 0)
					sHTML += "No answer";
				else
					sHTML += q.rgsAnswers[q.nGivenAnswer - 1];
					
				sHTML += "<br>";

				if (q.nGivenAnswer == q.nCorrectAnswer)
					{
					sHTML += "<span style='color:blue;'><b>GOOD CALL</b></span><br>";
					}
				else
					{
					sHTML += "<span style='color:red;'><b>OH NO! BLOWN CALL!</b></span><br>";
					sHTML += "<b>Correct answer:</b> " + q.rgsAnswers[q.nCorrectAnswer - 1] + "<br>";
					}
				sHTML += "<b>Ruling: </b>" + q.sRuling;
				}
			sHTML += "</table>";
			return sHTML + "<br clear=all><hr width=100%>";
		}

		/* S  G E N E R A T E  H T M L  F O R  Q U E S T I O N */
		/*----------------------------------------------------------------------------
			%%Function: SGenerateHtmlForQuestion
			%%Qualified: QuizSupp.Support.SGenerateHtmlForQuestion
			%%Contact: rlittle

			
		----------------------------------------------------------------------------*/
		public string SGenerateHtmlForQuestion(int iQuestion)
		{
			return SGenerateHtmlForQuestion((Question)plQuestions[iQuestion], iQuestion);
		}

		/* S  G E N E R A T E  S C O R E */
		/*----------------------------------------------------------------------------
			%%Function: SGenerateScore
			%%Qualified: QuizSupp.Support.SGenerateScore
			%%Contact: rlittle

			
		----------------------------------------------------------------------------*/
		public string SGenerateScore()
		{
			int cCorrect = 0;

			if (plQuestions == null)
				return "No questions answered, no score!";
				
			foreach (Question q in plQuestions)
				{
				if (q.nCorrectAnswer == q.nGivenAnswer)
					cCorrect++;
				}

			return String.Format("<b>{0}</b> Questions.<br><b><font color=blue>{1} Good Calls, </b></font><font color=red><b>{2} Blown Calls</b></font>", plQuestions.Count, cCorrect, plQuestions.Count - cCorrect);

		}

		/* S  G E N E R A T E  H T M L  F O R  A L L  Q U E S T I O N S */
		/*----------------------------------------------------------------------------
			%%Function: SGenerateHtmlForAllQuestions
			%%Qualified: QuizSupp.Support.SGenerateHtmlForAllQuestions
			%%Contact: rlittle

			
		----------------------------------------------------------------------------*/
		public string SGenerateHtmlForAllQuestions()
		{
			string s = "";
			int iQuestion = 0;
			if (plQuestions == null)
				return "";
				
			foreach (Question q in plQuestions)
				{
				s += SGenerateHtmlForQuestion(q, iQuestion);
				iQuestion++;
				}

			return s;
		}
	}

}

