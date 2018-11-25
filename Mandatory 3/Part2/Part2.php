<?php

//Aaron Reboredo Vázquez
//Pablo Martín García

error_reporting(E_ALL);

/* Permitir al script esperar para conexiones. */
set_time_limit(0);

/* Activar el volcado de salida implícito, así veremos lo que estamo obteniendo
* mientras llega. */
ob_implicit_flush();

$address = '127.0.0.1';
$port = 10000;

$actualNumberOfClients = 0;
$maxNumberOfClients = 5;

if (($sock = socket_create(AF_INET, SOCK_STREAM, SOL_TCP)) === false) {
    echo "socket_create() failed: reason: " . socket_strerror(socket_last_error()) . "\n";
}

if (socket_bind($sock, $address, $port) === false) {
    echo "socket_bind() failed: reason: " . socket_strerror(socket_last_error($sock)) . "\n";
}

if (socket_listen($sock, 5) === false) {
    echo "socket_listen() failed: reason: " . socket_strerror(socket_last_error($sock)) . "\n";
}

//clients array
$clients = array();

do {
    $read = array();
    $read[] = $sock;
    
    $read = array_merge($read,$clients);
    
    // Set up a blocking call to socket_select

    $write = NULL;
    $except = NULL;
    $tv_sec = 5;

    if(socket_select($read, $write, $except, $tv_sec) < 1)
    {
            //SocketServer::debug("Problem blocking socket_select?");
        continue;
    }
    
    // Handle new Connections
    
    if (in_array($sock, $read)) {        
        
        if (($msgsock = socket_accept($sock)) === false) {
            echo "socket_accept() failed: reason: " . socket_strerror(socket_last_error($sock)) . "\n";
            break;
        }

        //manage connections with a maximum number of conections
        if ($actualNumberOfClients < $maxNumberOfClients) {
            $clients[] = $msgsock;
            $key = array_keys($clients, $msgsock);
            /* Enviar instrucciones. */
            $msg = "\nWelcome to the PHP Test Server. \n" .
            "Your client number is : {$key[0]}\n" .
            "To quit, type 'quit'. To shut down the server type 'shutdown'.\n \n";
            
            $actualNumberOfClients++;
            echo "\n New client has joined the server \n";
            echo "There are {$actualNumberOfClients} clients conected \n\n";
            socket_write($msgsock, $msg, strlen($msg));
        }
        else{
            $msg = "Maximum number of clients on the server reached, please try later";
            socket_write($msgsock, $msg, strlen($msg));
            unset($msgsock); // ESTO LO DESCONECTA (DESTRUYE)
        }        
    }
    
    // Handle Input
    foreach ($clients as $key => $client) { // for each client        
        if (in_array($client, $read)) {

            if (false === ($buf = socket_read($client, 2048, PHP_NORMAL_READ))) {
                echo "socket_read() failed: reason: " . socket_strerror(socket_last_error($client)) . "\n";
                break 2;
            }
            if (!$buf = trim($buf)) {
                continue;
            }
            if ($buf == 'quit') {
                unset($clients[$key]);
                socket_close($client);

                $actualNumberOfClients--;
                echo "\n Client {$key} left the server \n";
                echo "Current number of clients conected : {$actualNumberOfClients} \n \n";
                break;
            }
            if ($buf == 'shutdown') {

                echo "Client {$key} shut down the server \n";

                socket_close($client);
                break 2;
            }

//posting the data with the message on the server and for the clients who recieve the message

            $timezone = date_default_timezone_get();
            date_default_timezone_set($timezone);
            $date = date('m/d/Y h:i:s a', time());

            echo "Client number {$key} said :  $buf at $date \n";



            $sender = $key;
            foreach ($clients as $key => $client){
                if(!($key === $sender) ){

                    //the other clients recieve the message with the date and hour
                $talkback = "Client number {$sender }: said '$buf' at '$date'.\n";
                socket_write($client, $talkback, strlen($talkback));
                }
            }
        }
    }        
} while (true);
socket_close($sock);
?>