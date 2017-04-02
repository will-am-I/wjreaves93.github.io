$('#codeexample').ready(function()
{
	$('#example').ready(function()
	{
		$('#agenterroraspx').click(function()
		{
			$('#codeexample').css('display', 'block');
			
			$.ajax(
			{
				url: 'files/agenterror.aspx',
				dataType: 'text',
				success: function(data)
				{
					$('#example').empty();
					$('#example').text(data);
				}
			});
		});
		
		$('#agenterrorcs').click(function()
		{
			$('#codeexample').css('display', 'block');
			
			$.ajax(
			{
				url: 'files/agenterror.aspx.cs',
				dataType: 'text',
				success: function(data)
				{
					$('#example').empty();
					$('#example').text(data);
				}
			});
		});
		
		$('#rosterphp').click(function()
		{
			$('#codeexample').css('display', 'block');
			
			$.ajax(
			{
				url: 'files/roster.php',
				dataType: 'text',
				success: function(data)
				{
					$('#example').empty();
					$('#example').text(data);
				}
			});
		});
		
		$('#rostercss').click(function()
		{
			$('#codeexample').css('display', 'block');
			
			$.ajax(
			{
				url: 'files/roster.css',
				dataType: 'text',
				success: function(data)
				{
					$('#example').empty();
					$('#example').text(data);
				}
			});
		});
		
		$('#scoreboardhtml').click(function()
		{
			$('#codeexample').css('display', 'block');
			
			$.ajax(
			{
				url: 'files/scoreboard.html',
				dataType: 'text',
				success: function(data)
				{
					$('#example').empty();
					$('#example').text(data);
				}
			});
		});
		
		$('#adventurephp').click(function()
		{
			$('#codeexample').css('display', 'block');
			
			$.ajax(
			{
				url: 'files/adventure.php',
				dataType: 'text',
				success: function(data)
				{
					$('#example').empty();
					$('#example').text(data);
				}
			});
		});
		
		$('#hordephp').click(function()
		{
			$('#codeexample').css('display', 'block');
			
			$.ajax(
			{
				url: 'files/horde.php',
				dataType: 'text',
				success: function(data)
				{
					$('#example').empty();
					$('#example').text(data);
				}
			});
		});
	});
});