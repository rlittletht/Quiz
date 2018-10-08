<%@ Page language="c#" Codebehind="Quiz.aspx.cs" AutoEventWireup="false" Inherits="quiz.WebForm1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
<script>
    (function (i, s, o, g, r, a, m) {
        i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
            (i[r].q = i[r].q || []).push(arguments)
        }, i[r].l = 1 * new Date(); a = s.createElement(o),
        m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
    })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

    ga('create', 'UA-43121510-1', 'thetasoft.com');
    ga('send', 'pageview');

</script>
		<title>Little League Umpire Quiz</title>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style>
		    Div.QuizIntro 
		    {
		        text-align: left;
		        font-family: Verdana;
		        font-size: 10pt;
	        }
		</style>
		<script language="JavaScript">
			var vsLeague = "<%=Request.QueryString["league"]%>";
			var vsCount = "<%=Request.QueryString["count"]%>";
			var vsLLOnly = "<%=Request.QueryString["llonly"]%>";

			var vcCount = (vsCount == "" ? 0 : 0 + vsCount);
			var vfLLOnly = (vsLLOnly == "" ? false : true);

			/* G E T  C B X  V A L */
			/*----------------------------------------------------------------------------
				%%Function: GetCbxVal
				%%Contact: rlittle

				get a value from a combobox (dropdownlist)	
			----------------------------------------------------------------------------*/
			function GetCbxVal(sid)
			{
				var o = document.getElementById(sid);
				if (o.length == 0 || o.selectedIndex < 0)
					return "";

				return o.options[o.selectedIndex].value;
			}


			/* G E T  C B X  T E X T */
			/*----------------------------------------------------------------------------
				%%Function: GetCbxText
				%%Contact: rlittle

			----------------------------------------------------------------------------*/
			function GetCbxText(sid)
			{
				var o = document.getElementById(sid);
				if (o.length == 0 || o.selectedIndex < 0)
					return "";

				return o.options[o.selectedIndex].text;
			}

			function FSetCbxSelection(sid, sVal)
            {
                var o = document.getElementById(sid);
                if (o.length == 0)
                    return "";

				var i = 0;
				while (i < o.length)
                    {
					if (o.options[i].value == sVal)
                        {
						o.selectedIndex = i;
						return true;
                        }
					i++;
                    }
				return false;
            }

			function SetFormVal(sid, sVal)
            {
                var o = document.getElementById(sid);

				o.value = sVal;
            }

			function AddCbxOptionSelect(sid, sVal, sText)
            {
                var o = document.getElementById(sid);

				o.add(new Option(sVal, sText, null));
            }

			function GetCbVal(sid)
			{
				var o = document.getElementById(sid);

				return o.checked;
			}

			function OnLoadComplete()
            {
				SetFormVal("idQuizParms", "<%=QueryString()%>");

                if (vfLLOnly == true)
                    {
					var oTrFed = document.getElementById("idTrFedRules");

					oTrFed.style.visibility = "hidden";
                    }

				if (vsLeague != "")
                    {
                    FSetCbxSelection("idSelect", vsLeague);
                    }

				if (vcCount != 0)
                    {
					if (!FSetCbxSelection("idCount", vsCount))
                        {
						AddCbxOptionSelect("idCount", vsCount, vsCount);
                        FSetCbxSelection("idCount", vsCount);
                        }
                    }
            }

		</script>
	</HEAD>
	<body lang="en-us" onload="OnLoadComplete()">
		<P align="center"><FONT face="Verdana" color="#0000ff" size="5">
				<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0" bgColor="darkgray">
					<TR>
						<TD><IMG alt="" src="baseball.png" ></TD>
						<TD>
							<P align="center"><IMG alt="" src="title.gif" ></P>
							<P align="center">Questions based on NFHS, OBR, and Little League rules.</P>
						</TD>
						<TD align="right"><IMG alt="" src="baseball.png" ></TD>
					</TR>
				</TABLE>
			</FONT>
		</P>
		<DIV class="QuizIntro">
			<P>
				This interactive quiz has over 300 questions (and growing).&nbsp; These questions have been collected from all
				over the place. Feel free to send more my way!
			</P>
			<P> The database of questions started
				out as a learning aid for umpires working Little League games at the majors and
				below.&nbsp; Over the years, its expanded to included Junior/Senior/Big League and
				Softball.&nbsp; This year, the database expands yet again to include Federation
				High School rules (NFHS)!
			</P>
			<p>
				For each question, I try to cite the rule.&nbsp; When the question was written (barring
				dyslexia), the correct rule was cited.&nbsp; As rule books change, the rule numbers
				sometimes drift, so you might have to look around a little to find the exact rule.&nbsp; If a ruling is the same for
				Fed and OBR, I only cite the OBR rule.&nbsp; If there's something special for Fed,
				I'll cite the Fed rule.
			</p>
			<p>
				Some of these questions are specific to High School or Little League, a particular level of Little League or even
				a specific local Little League.  You can choose to filter out these questions using the options below.  If a question 
				doesn't apply to all basball/softball, it will note what it does apply to.&nbsp; (If nothing is noted, the rule is 
				the same for all associations, all levels)
			</p>
			<p>
				If you find any of these questions in error (or you just want to drop me a line), send me mail at </font>
				<A href="mailto:redmond_blue@yahoo.com">redmond_blue@yahoo.com</A>.
			</p>
		</DIV>
		<hr>
		<P></P>
		<form id="Form1" action="GenQuiz.aspx" method="post">
			<table >
			<tr>
			<td colspan=2 style="height: 21px">
			<P align="left"><STRONG>Instructions:</STRONG>&nbsp; Choose the number of questions 
				you want, then click "Generate Quiz".&nbsp; Good luck!</P>
			</td>
			<td valign="top" rowspan=3>

                <p style="margin-left: 0.5in">
                    <strong>Definitions</strong>
                </p>
			<table style="margin-left: .5in" cellpadding="2" border="1">
			    <tr>
				<td>BR</td>
				<td>Batter/Runner</td>
				</tr>
				<tr>
					<td>R1</td>
					<td>Runner starting at 1st base</td>
				</tr>
				<tr>
					<td>R2</td>
					<td>Runner starting at 2nd base</td>
				</tr>
				<tr>
					<td>R3</td>
					<td>Runner starting at 3rd base</td>
				</tr>
			</table></td>
			</tr>
		    <tr>
			<td>
		    <div style="margin-left: .25in">
			<P>Number of questions:&nbsp;&nbsp;&nbsp;
				<SELECT id="idCount" name="COUNT">
					<OPTION value="3">3</OPTION>
					<OPTION value="5" selected>5</OPTION>
					<OPTION value="10">10</OPTION>
					<OPTION value="15">15</OPTION>
				    <OPTION value="25">25</OPTION>
				    <OPTION value="50">50</OPTION>
				</SELECT></P>
				<div>Include questions for: (check all that apply)
				<div style="margin-left:.5in">
				<table border=0 cellpadding=3 ><tr>
				<tr>
				<td><input type=checkbox checked name="Minors"/>
                    Little League Minors<td><input type=checkbox checked name="Majors"/>
                    Little League
                    Majors<tr>
				<td><input type=checkbox checked name="JrSrBl"/>
                    Junior/Senior/Big League<td><input type=checkbox checked name="Softball"/>
                        Little League
                    Softball</tr>
				<tr id="idTrFedRules" runat="server">
				<td><input type=checkbox checked name="Fed"/>
                    NFHS (<i>Fed/High School</i>)</td>
				</tr>
				</table>
			</div>
			</div>
                <br />
			<div >
                Difficulty (choose one more more levels of difficulty)
			<div style="margin-left:.5in">
				<table border=0 cellpadding=3 ><tr>
				<tr>
				<td><input type=checkbox checked name="Diff1"/>
                    Umpire "101" (The absolute basics)
				<td><input type=checkbox checked name="Diff2"/>
                    Rookie (for deeper coverage of the basics)
				<tr>
				<td><input type=checkbox  name="Diff3"/>
                    Senior (more complicated situtations)
				<td><input type=checkbox  name="Diff4"/>
                    Veteran (difficult interpretations/situations)
				</tr>
				</table>
				</div></div>
				<p >
				Include local league questions for: <select id="idSelect" name="Local">
				    <OPTION value="None">No local rules</OPTION>
				    <OPTION value="Redmond West">Redmond West</OPTION>
				    <OPTION value="Redmond North">Redmond North</OPTION>				    				    
				</select>
				Q-Debug-List: <input type="text" name="QDebug" />
				<input type="hidden" name="quizParms" id="idQuizParms">
				</p>
			</div>
			</td>
	        <tr>
	        <td>
			<P align=right>
				<INPUT type="submit" value="Generate Quiz"></P>
			</table>
			<br>
		</form>
		
		<span style="BACKGROUND:green; WIDTH:100%; COLOR:white; BOTTOM:0px; aPOSITION:absolute; HEIGHT:4px">
			<b>&nbsp;<%=SCount()%>
				Quizzes served to date...</b></span> <img src="umpire.gif" style="RIGHT:0px;BOTTOM:0px;POSITION:absolute">
	</body>
</HTML>
