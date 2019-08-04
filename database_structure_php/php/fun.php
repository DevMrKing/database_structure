<?php
/**
 * 数据库操作
 */
require_once('mysql_db.php');
require_once('postgresql_db.php');
require_once('sqlserver_db.php');
require_once('oracle_db.php');

const dbms_mysql='mysql';
const dbms_pgsql='postgresql';
const dbms_oracle='oracle';
const dbms_sqlserver='sqlserver';

/**
 * 测试连接
 */
function db_test_connect(){
    $data=null;
    try {
        if($_POST['dbms']==dbms_mysql){
            if(test_mysql_connect()){
                echo json_encode(array("success"=>true,"msg"=>'',"data"=>$data));
            }
        }else if($_POST['dbms']==dbms_pgsql){
            if(test_postgresql_connect()){
                echo json_encode(array("success"=>true,"msg"=>'',"data"=>$data));
            }
        }else if($_POST['dbms']==dbms_sqlserver){
            if(test_sqlserver_connect()){
                echo json_encode(array("success"=>true,"msg"=>'',"data"=>$data));
            }
        }else if($_POST['dbms']==dbms_oracle){
            if(test_oracle_connect()){
                echo json_encode(array("success"=>true,"msg"=>'',"data"=>$data));
            }
        }
    } catch (PDOException $e) {
       echo json_encode(array("success"=>false,"msg"=>"".$e->getMessage(),"data"=>$data));
    }
}

/**
 * 获得数据库的所有表
 */
function db_tables(){
    $data=null;
    try {
        if($_POST['dbms']==dbms_mysql){
            $data=mysql_tables();
        }else if($_POST['dbms']==dbms_pgsql){
            $data=postgresql_tables();
        }else if($_POST['dbms']==dbms_sqlserver){
            $data=sqlserver_tables();
        }else if($_POST['dbms']==dbms_oracle){
            $data=oracle_tables();
        }
        echo json_encode(array("success"=>true,"msg"=>'',"data"=>$data));
    } catch (PDOException $e) {
        echo json_encode(array("success"=>false,"msg"=>$e->getMessage(),"data"=>$data));
    }
}

/*
 * 获得数据库的选中的表结构
 */
function db_generation(){
    $data=null;
    try {
        if($_POST['dbms']==dbms_mysql){
            $data=mysql_generation();
        }else if($_POST['dbms']==dbms_pgsql){
            $data=postgresql_generation();
        }else if($_POST['dbms']==dbms_sqlserver){
            $data=sqlserver_generation();
        }else if($_POST['dbms']==dbms_oracle){
            $data=oracle_generation();
        }
        echo json_encode(array("success"=>true,"msg"=>'',"data"=>$data));
    } catch (PDOException $e) {
        echo json_encode(array("success"=>false,"msg"=>$e->getMessage(),"data"=>$data));
    }
}

/**
 * 另存为html文件
 */
function save_html(){
    try {
        $file_content='';
        if($_POST['dbms']==dbms_mysql){
            $file_content=mysql_save_html();
        }else if($_POST['dbms']==dbms_pgsql){
            $file_content=postgresql_save_html();
        }else if($_POST['dbms']==dbms_sqlserver){
            $file_content=sqlserver_save_html();
        }else if($_POST['dbms']==dbms_oracle){
            $file_content=oracle_save_html();
        }
        //替换文件公共内容
        $file_content=str_replace('#dbms#',$_POST['dbms'],$file_content);
        $file_content=str_replace('#dbname#',$_POST['dbname'],$file_content);
        $file_content=str_replace('#tableCols#',$_POST['cols'],$file_content);
        $file_content=str_replace('#search_input_placeholder#',$_POST['search_input_placeholder'],$file_content);
        //下载文件
        $fname=$_POST['dbms'].'_'.$_POST['dbname'].'_'.time().'.html';
        download($fname,$file_content);
    } catch (Exception $e) {
        
    }
}

/**
 * 下载文件
 * @param $fname 下载的文件名
 * @param $fpath 读取文件的路径
 */
function download($filename,$content){
    //返回的文件(流形式)
    header("Content-type: application/octet-stream");
    //按照字节大小返回
    header("Accept-Ranges: bytes");
    //这里客户端的弹出对话框，对应的文件名
    header("Content-Disposition: attachment; filename=".$filename);
    ob_clean();
    flush();
    echo $content;//传数据给浏览器端
}

$method=$_POST['method'];
if($method=='test_connect'){
    db_test_connect();
}else if($method=='tables'){
    db_tables();
}else if($method=='generation'){
    db_generation();
}else if($method=='save_html'){
    save_html();
}
?>