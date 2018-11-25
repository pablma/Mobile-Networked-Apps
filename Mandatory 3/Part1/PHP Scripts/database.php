<?php

	$servername = "localhost";
	$username = "root";
	$password = "";
	$dbName = "Mandatory3_database";

	// Make connection
	$conn = new mysqli($servername, $username, $password, $dbName);
	// Check connection
	if(!$conn)
	{
		die("Connection failed." . mysqli_connect_error());
	}
	
	$sql = "SELECT User_id, Username, Password FROM users";
	$result = mysqli_query($conn, $sql);

	if(mysqli_num_rows($result) > 0)
	{
		while($row = mysqli_fetch_assoc($result))
		{
			echo "Id:" . $row['User_id'] . "|Username:" . $row['Username'] . "|Password:" . $row['Password'] . ";";
		}
	}
?>