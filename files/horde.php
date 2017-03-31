<?php
   if (file_exists('horde.txt'))
   {
      $file = fopen('horde.txt', 'r');
      
      if (filesize('horde.txt') > 0)
      {
         $scores = array(array());
         $count = 0;
         
         while (!feof($file))
         {
            if (fscanf($file, "%s %s %d %d %d %d", $fname, $lname, $time, $monkeys, $rounds, $score))
            {
               $scores[$count]['name'] = str_replace("_", " ", $fname." ".$lname);
               $scores[$count]['time'] = $time;
               $scores[$count]['monkeys'] = $monkeys;
               $scores[$count]['rounds'] = $rounds;
               $scores[$count]['score'] = $score;
               $count++;
            }
         }
         
         uasort($scores, 'sortScores');
         
         if ($count > 10)
         {
            $count = 10;
         }
         
         $line = 0;
         
         foreach ($scores as $score)
         {
            // End after top 10 are displayed
            if ($line == 10)
            {
               break;
            }
            
            // Print start for first place star
            if($line == 0)
            {
               echo "<tr style='background-color: #4CAF50; color: #fff; font-weight: bold;'><td><i class='material-icons' style='color: #FDD835'>stars</i></td>";
            }
            else
            {
               echo "<tr><td> </td>";
            }
            
            echo "<td>".++$line."</td>";
            echo "<td class='mdl-data-table__cell--non-numeric'>".ucwords(strtolower($score['name']))."</td>";
            echo "<td>".date('i:s', intval($score['time']))."</td>";
            echo "<td>".$score['monkeys']."</td>";
            echo "<td>".$score['rounds']."</td>";
            echo "<td>".$score['score']."</td>";
            echo "</tr>";
         }
      }
      else
      {
         echo "Awaiting results...";
      }
      
      fclose($file);
   }
   else
   {
      echo "Awaiting results...";
   }
   
   function sortScores ($x, $y)
   {
      if ($x['score'] == $y['score'])
      {
         return 0;
      }
      elseif ($x['score'] > $y['score'])
      {
         return -1;
      }
      else
      {
         return 1;
      }
   }
?>