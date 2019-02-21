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
					showExample(data);
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
					showExample(data);
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
					showExample(data);
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
					showExample(data);
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
					showExample(data);
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
					showExample(data);
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
					showExample(data);
				}
			});
		});
		
		$('#gamemanagercs').click(function()
		{
			$('#codeexample').css('display', 'block');
			
			$.ajax(
			{
				url: 'files/GameManager.cs',
				dataType: 'text',
				success: function(data)
				{
					showExample(data);
				}
			});
		});
		
		$('#shoppingcarthtml').click(function()
		{
			$('#codeexample').css('display', 'block');
			
			$.ajax(
			{
				url: 'files/shoppingcart.html',
				dataType: 'text',
				success: function(data)
				{
					showExample(data);
				}
			});
		});
		
		$('#shoppingcartphp').click(function()
		{
			$('#codeexample').css('display', 'block');
			
			$.ajax(
			{
				url: 'files/shoppingcart.php',
				dataType: 'text',
				success: function(data)
				{
					showExample(data);
				}
			});
		});
		
		$('#shoppingcartcss').click(function()
		{
			$('#codeexample').css('display', 'block');
			
			$.ajax(
			{
				url: 'files/shoppingcart.css',
				dataType: 'text',
				success: function(data)
				{
					showExample(data);
				}
			});
		});
	});
});

function showExample(data)
{
   $('#example').empty();
   $('#example').text(data);
   $('#codeexample').scrollTop(0);
}