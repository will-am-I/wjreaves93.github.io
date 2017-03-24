<!DOCTYPE HTML> 

<html>
	<head>
		<title>Team Player Listing</title>
		<meta name="author" content="Your name here" />
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8" /> 
		<link href="Roster.css" rel="stylesheet" type="text/css" />
		<link href="Images/sharks.png" rel="shortcut icon" />
		<?php
			define('FIRST_NAME', 1);
			define('LAST_NAME', 2);
			define('TIME_PLAYED', 3);
			define('SALARY', 4);
			
			//info taken from http://www.hockey-reference.com/teams/SJS/2017.html
			$team = array(39=>array('f_name'=>'Logan',
			                        'l_name'=>'Couture',
											'avg_time'=>1495,
											'salary'=>6000000),
							  42=>array('f_name'=>'Joel',
											'l_name'=>'Ward',
											'avg_time'=>953,
											'salary'=>3275000),
							   7=>array('f_name'=>'Paul',
								         'l_name'=>'Martin',
											'avg_time'=>1153,
											'salary'=>5000000),
							  27=>array('f_name'=>'Joonas',
											'l_name'=>'Donskoi',
											'avg_time'=>842,
											'salary'=>925000),
							   5=>array('f_name'=>'David',
											'l_name'=>'Schlemko',
											'avg_time'=>1000,
											'salary'=>2500000),
							  57=>array('f_name'=>'Tommy',
											'l_name'=>'Wingels',
											'avg_time'=>603,
											'salary'=>742500),
							  40=>array('f_name'=>'Ryan',
											'l_name'=>'Carpenter',
											'avg_time'=>623,
											'salary'=>600000),
							  83=>array('f_name'=>'Matthew',
											'l_name'=>'Nieto',
											'avg_time'=>733,
											'salary'=>735000),
							  82=>array('f_name'=>'Nikolay',
											'l_name'=>'Goldobin',
											'avg_time'=>590,
											'salary'=>925000),
							  31=>array('f_name'=>'Martin',
											'l_name'=>'Jones',
											'avg_time'=>3513,
											'salary'=>3000000));
		?>
	</head>
	<body>
		<h1><img src="Images/sharks.png" height="40" alt="Logo" />&nbsp;San Jose Sharks</h1>
		<h2>2016/17 Roster</h2>
		<form name="player_form" id="player_form" action="program03.php" method="post">
			<div class="player_form">
				<label>How would you like the sort this list? 
					<select name="ddl_sort" id="ddl_sort">
						<option value="1">First Name</option>   
						<option value="2">Last Name</option>
						<option value="3">Average Time Played</option>
						<option value="4">Salary</option>
					</select>
				</label>
				<input id="#button" type="submit" value="Sort Data" />
			</div>
		</form>
		<table id="roster">
			<tr>
				<th>
					Jersey
				</th>
				<th id="fname">
					First Name
				</th>
				<th id="lname">
					Last Name
				</th>
				<th title="Average Time On Ice" id="average">
					ATOI
				</th>
				<th id="salary">
					Salary
				</th>
			</tr>
			<?php
				if (isset($_POST['ddl_sort']))
				{
					switch ($_POST['ddl_sort'])
					{
						case FIRST_NAME:
							$function = 'sortFirstName';
							echo "<style type='text/css'>#fname{text-decoration:underline}</style>";
							break;
							
						case LAST_NAME:
							$function = 'sortLastName';
							echo "<style type='text/css'>#lname{text-decoration:underline}</style>";
							break;
							
						case TIME_PLAYED:
							$function = 'sortTimePlayed';
							echo "<style type='text/css'>#average{text-decoration:underline}</style>";
							break;
							
						case SALARY:
							$function = 'sortSalary';
							echo "<style type='text/css'>#salary{text-decoration:underline}</style>";
							break;
					}
					
					uasort($team, $function);
				}
				else
				{
					uasort($team, 'sortLastName');
				}
				
				function sortFirstName($x, $y)
				{
					if ($x['f_name'] == $y['f_name'])
					{
						return 0;
					}
					elseif ($x['f_name'] < $y['f_name'])
					{
						return -1;
					}
					else
					{
						return 1;
					}
				}
				
				function sortLastName($x, $y)
				{
					if ($x['l_name'] == $y['l_name'])
					{
						return 0;
					}
					elseif ($x['l_name'] < $y['l_name'])
					{
						return -1;
					}
					else
					{
						return 1;
					}
				}
				
				function sortTimePlayed($x, $y)
				{
					if ($x['avg_time'] == $y['avg_time'])
					{
						return 0;
					}
					elseif ($x['avg_time'] < $y['avg_time'])
					{
						return -1;
					}
					else
					{
						return 1;
					}
				}
				
				function sortSalary($x, $y)
				{
					if ($x['salary'] == $y['salary'])
					{
						return 0;
					}
					elseif ($x['salary'] < $y['salary'])
					{
						return -1;
					}
					else
					{
						return 1;
					}
				}
				
				foreach ($team as $number => $player)
				{
					echo "<tr><td class='number'>".$number."</td>";
					echo "<td class='name'>".$player['f_name']."</td>";
					echo "<td class='name'>".$player['l_name']."</td>";
					echo "<td class='time'>".date('i:s', intval($player['avg_time']))."</td>";
					echo "<td class='salary'>$".number_format(intval($player['salary']))."</td>";
					echo "</tr>";
				}
			?>
		</table>
		<br />
		<div id="gohome">
			<a id="home" href="../index.html">Return to Home</a>
		</div>
	</body>
</html>