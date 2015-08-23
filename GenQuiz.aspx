<%@ Page language="c#" Codebehind="GenQuiz.aspx.cs" AutoEventWireup="false" Inherits="quiz.WebForm2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Interactive Umpire Quiz</title>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
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
		<P align="center"><STRONG><FONT size="5">Interactive&nbsp;Umpire Quiz</FONT></STRONG></P>
		<P>Answer the following questions by choosing the best answer from the choices 
			listed.&nbsp; When you are done, click "Score Quiz" to see how many Good Calls 
			you made.</P>
		<P>
			<hr>
		<P></P>
		<form id="WebForm2" action="QuizAnswers.aspx" method="post">
			<%=oSupp.SGenerateHtmlForAllQuestions()%>
			<INPUT type="hidden" name="quizParms" value="<%=SGetQuizParms()%>">
			<INPUT id="Submit1" type="submit" value="Score Quiz" name="Submit1">
		</form>
	</body>
</HTML>
