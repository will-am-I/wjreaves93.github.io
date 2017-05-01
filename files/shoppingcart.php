<?php
   if (isset($_GET['button']))
   {
      if ($_GET['button'] == 'cart')
      {
         $userName = trim(substr($_SERVER['AUTH_USER'], 11));
         
         if (isset($_GET['productid']))
         {
            $productId = $_GET['productid'];
            
            if (productExists($productId))
            {
               if (productInCart($productId, $userName))
               {
                  echo "<script type='text/javascript'>alert('This product is already in your cart.\\nPlease update the quantity instead.');</script>";
                  
                  showProducts();
               }
               else
               {
                  insertRow($productId, $userName);
                  
                  showCart($userName);
               }
            }
            else
            {
               echo "<script type='text/javascript'>alert('This product does not exist.');</script>";
                  
                  showProducts();
            }
         }
         else if (isset($_GET['orderid']) && isset($_GET['quantity']))
         {
            $orderId = $_GET['orderid'];
            $quantity = $_GET['quantity'];
            
            if (orderExists($orderId))
            {
               if ($quantity > 0)
               {
                  updateQuantity($orderId, $quantity);
               }
               else
               {
                  deleteRow($orderId);
               }
               
               showCart($userName);
            }
            else
            {
               echo "<script type='text/javascript'>alert('This product is missing from your cart.\\nPlease add it to your cart.');</script>";
               
               showCart($userName);
            }
         }
         else
         {
            showCart($userName);
         }
      }
      else
      {
         showProducts();
      }
   }
   else
   {
      showProducts();
   }
   
   function showCart ($userName)
   {
      if (cartIsEmpty($userName))
      {
         echo "<p>Your cart is empty</p>";
      }
      else
      {
         $db = new mysqli("csweb", "117406", "pcc1910", "cs368_117406");
         
         $query = "SELECT p_picture, p_name, o_quantity, p_price, o_id, o_date
                     FROM orderinfo o
                     JOIN products p
                       ON p.p_id = o.p_id
                    WHERE o.o_username = ?
                    ORDER BY o.o_date";
         $stmt = $db->prepare($query);
         $stmt->bind_param("s", $userName);
         $stmt->execute();
         $stmt->bind_result($picture, $name, $quantity, $price, $orderId, $date);
         
         $total = 0;
         
         echo "<table id='cart'><tr><th>&nbsp;</th><th>Name</th><th>Quantity</th><th>Price</th><th>Date Added</th><th>&nbsp;</th></tr>";
         while ($stmt->fetch())
         {
            echo "<tr>";
            echo "<td class='picture'><img src='data:image/jpeg;base64,".base64_encode($picture)."' height='55px'/></td>";
            echo "<td class='name'>".$name."</td>";
            echo "<td class='quantity'><input type='number' id='txtQuantity".$orderId."' name='txtQuantity".$orderId."' value='".$quantity."' /></td>";
            echo "<td class='price'>$".number_format(floatval($price), 2, ".", ",")."</td>";
            echo "<td class='date'>".date('M. d, Y', strtotime($date))."</td>";
            echo "<td class='update'><button type='submit' onclick='updateQuantity(".$orderId.")'>Update Quantity</button></td>";
            echo "</tr>";
            
            $total += floatval($price) * floatval($quantity);
         }
         echo "<tr id='total'><td colspan='2'></td><td class='total'>Total:</td><td class='totalPrice'>$".number_format($total, 2, ".", ",")."</td><td colspan='2'></td></tr></table>";
      
         $stmt->close();
      }
   }
   
   function cartIsEmpty($userName)
   {
      $db = new mysqli("csweb", "117406", "pcc1910", "cs368_117406");
      
      $query = "SELECT * FROM orderinfo WHERE o_username = ?";
      $results = $db->prepare($query);
      $results->bind_param("s", $userName);
      $results->execute();
      $results->store_result();
      
      if ($results->num_rows > 0)
      {
         $emptyCart = false;
      }
      else
      {
         $emptyCart = true;
      }
      
      $results->close();
      
      return $emptyCart;
   }
   
   function insertRow($productId, $userName)
   {
      $status = 'In Cart';
      $quantity = 1;
      $date = date('Y-m-d');
      
      $db = new mysqli("csweb", "117406", "pcc1910", "cs368_117406");
      
      $query = "INSERT INTO orderinfo (o_status, o_username, o_date, p_id, o_quantity) VALUES (?, ?, ?, ?, ?)";
      $stmt = $db->prepare($query);
      $stmt->bind_param("sssdd", $status, $userName, $date, $productId, $quantity);
      $stmt->execute();
      
      if ($stmt->affected_rows > 0)
      {
         echo "row inserted";
      }
      
      $stmt->close();
   }
   
   function updateQuantity($orderId, $quantity)
   {
      $db = new mysqli("csweb", "117406", "pcc1910", "cs368_117406");
      
      $query = "UPDATE orderinfo SET o_quantity = ? WHERE o_id = ?";
      $stmt = $db->prepare($query);
      $stmt->bind_param("dd", $quantity, $orderId);
      $stmt->execute();
      
      $stmt->close();
   }
   
   function deleteRow($orderId)
   {
      $db = new mysqli("csweb", "117406", "pcc1910", "cs368_117406");
      
      $query = "DELETE FROM orderinfo WHERE o_id = ?";
      $stmt = $db->prepare($query);
      $stmt->bind_param("d", $orderId);
      $stmt->execute();
      
      $stmt->close();
   }
   
   function showProducts()
   {
      $db = new mysqli("csweb", "117406", "pcc1910", "cs368_117406");
      
      $query = "SELECT * FROM products ORDER BY p_description DESC, p_name";
      $results = $db->query($query);
      
      echo "<table id='products'><tr><th>&nbsp;</th><th>Name</th><th>Description</th><th>Price</th><th>Year Approved</th><th>&nbsp;</th></tr>";
      for ($i = 1; $i <= $results->num_rows; $i++)
      {
         $data = $results->fetch_assoc();
         
         echo "<tr>";
         echo "<td class='picture'><img src='data:image/jpeg;base64,".base64_encode($data['P_PICTURE'])."' height='75px'/></td>";
         echo "<td class='name'>".$data['P_NAME']."</td>";
         echo "<td class='description'>".$data['P_DESCRIPTION']."</td>";
         echo "<td class='price'>$".number_format(floatval($data['P_PRICE']), 2, ".", ",")."</td>";
         echo "<td class='year'>".$data['P_YEAR']."</td>";
         echo "<td class='insert'><button type='submit' onclick='addToCart(".$data['P_ID'].")'>Add to Cart</button></td>";
         echo "</tr>";
      }
      echo "</table>";
      
      $results->free();
   }
   
   function orderExists($orderId)
   {
      $db = new mysqli("csweb", "117406", "pcc1910", "cs368_117406");
      
      $query = "SELECT * FROM orderinfo WHERE o_id = ?";
      $results = $db->prepare($query);
      $results->bind_param('d', $orderId);
      $results->execute();
      $results->store_result();
      
      if ($results->num_rows == 0)
      {
         $exists = false;
      }
      else
      {
         $exists = true;
      }
      
      $results->close();
      
      return $exists;
   }
   
   function productExists($productId)
   {
      $db = new mysqli("csweb", "117406", "pcc1910", "cs368_117406");
      
      $query = "SELECT * FROM products WHERE p_id = ?";
      $results = $db->prepare($query);
      $results->bind_param('d', $productId);
      $results->execute();
      $results->store_result();
      
      if ($results->num_rows == 0)
      {
         $exists = false;
      }
      else
      {
         $exists = true;
      }
      
      $results->close();
      
      return $exists;
   }
   
   function productInCart($productId, $userName)
   {
      $db = new mysqli("csweb", "117406", "pcc1910", "cs368_117406");
      
      $query = "SELECT * FROM orderinfo WHERE p_id = ? AND o_username = ?";
      $results = $db->prepare($query);
      $results->bind_param('ds', $productId, $userName);
      $results->execute();
      $results->store_result();
      
      if ($results->num_rows == 0)
      {
         $inCart = false;
      }
      else
      {
         $inCart = true;
      }
      
      $results->close();
      
      return $inCart;
   }
?>