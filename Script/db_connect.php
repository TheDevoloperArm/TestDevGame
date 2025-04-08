<?php
$host = "103.91.190.179";
$dbname = "testdev04";
$username = "testdev04";
$password = "l5ya0kLv1ElUonYF9uejHpzincihLFz0";

try {
  $conn = new PDO("mysql:host=$host;dbname=$dbname;charset=utf8", $username, $password);
  // ตั้งค่า Error Mode เป็น Exception
  $conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
  // echo "เชื่อมต่อฐานข้อมูลสำเร็จ";
} catch (PDOException $e) {
  echo "การเชื่อมต่อล้มเหลว: " . $e->getMessage();
  exit;
}
