$(document).ready(function()
{
	$('#header').ready(function()
	{
		$('#header').load('pages/header.html');
	});

	$('#footer').ready(function()
	{
		$('#footer').load('pages/footer.html');
	});
});

$(document).on('resize', 'window', function()
{
	var height = $(window).height();
	var width = $(window).width();
	
	if (height > width)
	{
		$('#nav').css('display', 'none');
		$('#headnav').css('display', 'block');
		$('#hamburger').css('display', 'block');
	}
	else
	{
		$('#nav').css('display', 'block');
		$('#headnav').css('display', 'block');
		$('#hamburger').css('display', 'none');
	}
});