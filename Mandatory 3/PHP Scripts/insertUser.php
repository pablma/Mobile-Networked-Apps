<?php

	$servername = "localhost";
	$server_username = "root";
	$server_password = "";
	$dbName = "mandatory3_database";

	$username = $_POST["usernamePost"];
	$password = $_POST["passwordPost"]; 

	// Make connection
	$conn = new mysqli($servername, $server_username, $server_password, $dbName);
	// Check connection
	if(!$conn)
	{
		die("Connection failed." . mysqli_connect_error());
	}
	
	echo "Everything is ok";
	/	
	$sql = "INSERT INTO users (Username, Password)
			VALUES ('" . $username . "', '" . $password . "')";
	
	$result = mysqli_query($conn, $sql);

	if(!$result)
		echo "there was an error";
	else
		echo "User created!"
		

?>