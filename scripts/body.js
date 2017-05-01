$(document).ready(function()
{
	var mobile = false;
	var navOpen = false;
	
	$('#header').on('click', '#hamburger', function()
	{
		if (navOpen)
		{
			$('#nav').css({'max-height': '0', 'transition': 'max-height 1.5s ease-out'});
			$('#nav').css('display', 'none');
			navOpen = false;
		}
		else
		{
			$('#nav').css('display', 'block');
			$('#nav').css({'max-height': '300px', 'transition': 'max-height 1.5s ease-in'});
			navOpen = true;
		}
		
		mobile = true;
	});
	
	$('#header').on('click', '#about', function()
	{
		$('#body').empty();
		$('#body').load('pages/about.html');
		
		if (mobile)
		{
			$('#nav').css({'max-height': '0', 'transition': 'max-height 1.5s ease-out'});
			$('#nav').css('display', 'none');
			navOpen = false;
		}
	});

	$('#header').on('click', '#portfolio', function()
	{
		$('#body').empty();
		$('#body').load('pages/portfolio.html');
		
		if (mobile)
		{
			$('#nav').css({'max-height': '0', 'transition': 'max-height 1.5s ease-out'});
			$('#nav').css('display', 'none');
			navOpen = false;
		}
	});

	$('#header').on('click', '#resume', function()
	{
		$('#body').empty();
		$('#body').load('pages/resume.html');
		
		if (mobile)
		{
			$('#nav').css({'max-height': '0', 'transition': 'max-height 1.5s ease-out'});
			$('#nav').css('display', 'none');
			navOpen = false;
		}
	});

	$('#header').on('click', '#clients', function()
	{
		$('#body').empty();
		$('#body').load('pages/clients.html');
		
		if (mobile)
		{
			$('#nav').css({'max-height': '0', 'transition': 'max-height 1.5s ease-out'});
			$('#nav').css('display', 'none');
			navOpen = false;
		}
	});
});