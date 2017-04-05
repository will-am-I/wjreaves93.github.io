$(document).ready(function()
{
	$('#header').on('click', '#about', function()
	{
		$('#body').empty();
		$('#body').load('pages/about.html');
	});

	$('#header').on('click', '#portfolio', function()
	{
		$('#body').empty();
		$('#body').load('pages/portfolio.html');
	});

	$('#header').on('click', '#resume', function()
	{
		$('#body').empty();
		$('#body').load('pages/resume.html');
	});

	$('#header').on('click', '#clients', function()
	{
		$('#body').empty();
		$('#body').load('pages/clients.html');
	});
});
