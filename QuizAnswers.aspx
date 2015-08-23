<%@ Page language="c#" Codebehind="QuizAnswers.aspx.cs" AutoEventWireup="false" Inherits="quiz.WebForm3" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Little League Quiz Answers</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
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
	</HEAD>
	<body>
		<form id="WebForm3" method="post" runat="server">
			<P align="center">
				<font size="5"><STRONG><FONT size="5">Interactive&nbsp;Little League Umpire Quiz Results</FONT></STRONG>
			</P>
			<p><%=oSupp.SGenerateScore()%></FONT>
				<hr>
				<%=oSupp.SGenerateHtmlForAllQuestions()%>
		</form>
		To take another quiz, <A href="quiz.aspx<%=SGetQuizParms()%>">click here.</A>
	</body>
</HTML>
