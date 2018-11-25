<?php

	$servername = "localhost";
	$server_username = "root";
	$server_password = "";
	$dbName = "mandatory3_database";

	$username = $_POST["usernamePost"];
	$password = $_POST["passwordPost"];;

	// Make connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	// Check connection
	if(!$conn)
	{
		die("Connection failed." . mysqli_connect_error());
	}
	
	$sql = "SELECT Password FROM users WHERE Username = '" . $username . "' ";
	$result = mysqli_query($conn, $sql);

	// Get the result and confirm login
	if(mysqli_num_rows($result) > 0)
	{
		while($row = mysqli_fetch_assoc($result))
		{
			if($row['Password'] == $password)
			{
				echo "Login success";
			}
			else
			{
				echo "Incorrect password";
			}
			
		}
	}
	else
	{
		echo "User not found";
	}
?>