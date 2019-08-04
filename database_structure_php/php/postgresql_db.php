<?php

/**
 * 获得pgsql-pdo的连接字符串
 */
function get_postgresql_dsn(){
    //数据库类型
    $dbms='pgsql';
    //数据库主机名
    $host=trim($_POST['host']);
    //使用端口
    $port=trim($_POST['port']);
    //使用的数据库
    $dbname=trim($_POST['dbname']);
    $dsn="$dbms:host=$host;port=$port;dbname=$dbname";
    return $dsn;
}

/**
 * 获取pgsql-数据库连接
 */
function get_postgresql_connect(){
    //初始化一个PDO对象
    $dsn=get_postgresql_dsn();
    //数据库连接用户名
    $user=trim($_POST['user']);
    //对应的密码
    $pass=trim($_POST['pass']);
    //设置编码格式
    $connect = new PDO($dsn,$user,$pass);
    return $connect;
}

/**
 * 测试pgsql数据库连接
 * @return true:连接成功，false:连接失败
 */
function test_postgresql_connect(){
    //获取连接
    $connect=get_postgresql_connect();
    //测试连接
    $select = $connect->query('select count(1)');
    //获取返回值
    $meta = $select->getColumnMeta(0);
    return $meta>0;
}

/**
 * 获得pgsql数据库的所有表
 */
function postgresql_tables(){
    //获取连接
    $connect=get_postgresql_connect();
    //设置模式
    $connect->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    //查询sql
    $sql="SELECT tb.tablename AS table_name,cast(obj_description(c.relfilenode,'pg_class') AS varchar) AS table_comment 
    FROM pg_tables tb
    LEFT JOIN pg_class c ON tb.tablename=relname
    WHERE schemaname = 'public'";
    //预处理sql
    $select=$connect->prepare($sql);
    //查询变量
    $select->execute();
    //返回数据样式
    $data = $select->fetchAll(PDO::FETCH_OBJ);
    return $data;
}

/**
 * 获得pgsql数据库的所有表结构
 */
function postgresql_generation(){
    //获取连接
    $connect=get_postgresql_connect();
    //设置模式
    $connect->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    //查询sql
    $sql_tb="SELECT tb.tablename AS table_name,cast(obj_description(c.relfilenode,'pg_class') AS varchar) AS table_comment 
    FROM pg_tables tb
    LEFT JOIN pg_class c ON tb.tablename=relname
    WHERE schemaname='public' AND tb.tablename IN (".$_POST["tb"].")";
    //预处理sql
    $select=$connect->prepare($sql_tb);
    //执行查询
    $select->execute();
    //返回所有表名有关的数据
    $tabls=$select->fetchAll(PDO::FETCH_OBJ);
    //循环处理所有表名，放入$tables总返回数据
    for ($i=0; $i<count($tabls); $i++) {
        //表的描述
        $sql_col="SELECT DISTINCT
        a.attnum as num,
        a.attname as column_name,
        format_type(a.atttypid, a.atttypmod) as column_type,
        a.attnotnull as is_nullable,
        com.description as column_comment,
        coalesce(i.indisprimary,false) as column_key,
        def.adsrc as column_default
        FROM pg_attribute a
        JOIN pg_class pgc ON pgc.oid = a.attrelid
        LEFT JOIN pg_index i ON (pgc.oid = i.indrelid AND i.indkey[0] = a.attnum)
        LEFT JOIN pg_description com ON (pgc.oid = com.objoid AND a.attnum = com.objsubid)
        LEFT JOIN pg_attrdef def ON (a.attrelid = def.adrelid AND a.attnum = def.adnum)
        WHERE a.attnum > 0 AND pgc.oid = a.attrelid
        AND pg_table_is_visible(pgc.oid)
        AND NOT a.attisdropped
        AND pgc.relname = ?
        ORDER BY a.attnum";
        //预处理sql
        $select=$connect->prepare($sql_col);
        //执行查询
        $select->execute(array($tabls[$i]->table_name));
        $tabls[$i]->colums=$select->fetchAll(PDO::FETCH_OBJ);
    }
    return $tabls;
}

/**
 * 另存为html文件
 */
function postgresql_save_html(){
    //表结构数据
    $tabls=postgresql_generation();
    //数据序列化
    $tableData=json_encode($tabls);
    //文件名
    $dir=dirname(__FILE__);
    $fpath=$dir.'/../template/postgresql/postgresql_save.html';
    //文件内容
    $handle = fopen($fpath, "r");
    //通过filesize获得文件大小，将整个文件读到一个字符串中
    $file_content = fread($handle, filesize ($fpath));
    fclose($handle);
    //替换内容
    $file_content=str_replace('#tableDataJson#',$tableData,$file_content);
    return $file_content;
}
?>