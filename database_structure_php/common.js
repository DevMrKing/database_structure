/**
 * jquery封装ajax的方法
 * @param opt 封装参数，使用var opt={url:'',data:''}等这种方式传递值
 */
function jqueryAjax(url,settings){
    //请求方式
    settings.method='post';
    //请求返回结果类型
    settings.dataType='json';
    //返回结果转换
    settings.converters={
        'text json': function(res) {
            return $.parseJSON(res);
        }
    };
    //请求完成的处理
    settings.complete=settings.complete || function(XMLHttpRequest, textStatus){

    };
    //成功处理
    var successCallBack=settings.success;
    settings.success=function(data, textStatus, jqXHR){
        if(successCallBack!=undefined){
            successCallBack(data,textStatus,jqXHR);
        }
    };
    //返回处理
    var errorCallBack=settings.error;
    settings.error=function(XMLHttpRequest, textStatus, errorThrown){
        if(errorCallBack!=undefined){
            errorCallBack(XMLHttpRequest,textStatus,errorThrown);
        }
    };
    $.ajax(url,settings);
}

//地址栏的hash监听转换
function hash_switch(hash){
    get_database_var();
    if(hash=="#language" || hash==""){
        //选择语言页面
        lang_html();
        return;
    }
    if(hash=="#connect"){
        //连接数据库页面
        connect_html();
        return;
    }
    if(window.dbms==undefined){
        location.href=location.pathname;
        return;
    }
    if(hash=="#table"){
        //选择表页面
        table_html();
        return;
    }
    if(hash=="#generation"){
        //选择生成表文档结构页面
        generation_html();
        return;
    }
}

//加载语言页面
function lang_html(){
    //加载语言选项
    $.get("view/language.html?tmp="+time_stamp(), function(html){
        var html = template.render(html, {'lang': window.lang,'lang_arr':lang_arr});
        $("#container").html(html);
    });
}

//选择语言后下一步操作
function lang_next() {
    //获取浏览器的语言
    var lang=$("#language").val();
    $.getScript("i18n/"+lang+".js", function(){
        localStorage.setItem(struct_data_storage_key+"lang",JSON.stringify(window.lang));
        $("title").html(window.lang.title);
        //加载语言选项
        location.href='#connect';
    });
}

//加载数据库连接页面
function connect_html(){
    $.get("view/connect.html?tmp="+time_stamp(), function(html){
        var html = template.render(html, {'lang': window.lang,'db':window,'support_database':support_database});
        $("#container").html(html);
    });
}

//获取页面数据库参数
function db_pm(){
    window.dbms=$("#dbms").val();
    window.host=$("#host").val();
    window.port=$("#port").val();
    window.dbname=$("#dbname").val();
    window.user=$("#user").val();
    window.pass=$("#pass").val();
    if(host==''){
        alert(lang.connect_form_validate.host_required_lbl);
        return false;
    }
    if(port==''){
        alert(lang.connect_form_validate.port_required_lbl);
        return false;
    }
    if(dbname==''){
        alert(lang.connect_form_validate.db_required_lbl);
        return false;
    }
    if(user==''){
        alert(lang.connect_form_validate.uname_required_lbl);
        return false;
    }
    if(pass==''){
        alert(lang.connect_form_validate.pwd_required_lbl);
        return false;
    }
    //写入缓存当中
    set_database_var()
    return true;
}

//测试数据库连接
function test_connect(){
    if(!db_pm()){
        return false;
    }
    //后台处理的方法
    var method='test_connect';
    //ajax提交
    var data={"dbms":window.dbms,"host":window.host,"port":window.port,"dbname":window.dbname,"user":window.user,"pass":window.pass,"method":method};
    var url="php/fun.php";
    var ajaxOpt={
        data:data,
        success:function(json){
            if(json.success){
                alert(window.lang.connect_form_validate.connect_success);
            }else{
                alert(json.msg);
            }
        }
    };
    jqueryAjax(url,ajaxOpt);
}

//获取数据库所有表
function get_tables(){
    if(!db_pm()){
        return false;
    }
    //后台处理的方法
    var method='tables';
    //ajax提交
    var data={"dbms":window.dbms,"host":window.host,"port":window.port,"dbname":window.dbname,"user":window.user,"pass":window.pass,"method":method};
    var url="php/fun.php";
    var ajaxOpt={
        data:data,
        success:function(json){
            localStorage.setItem(struct_data_storage_key+"tables",JSON.stringify(json));
            if(json.success){
                location.href='#table';
            }else{
                alert(json.msg);
            }
        }
    };
    jqueryAjax(url,ajaxOpt);
}

//显示所有表名的页面
function table_html(){
    var url="";
    if(window.dbms=='oracle'){
        url="template/oracle/oracle_tables.html?tmp="+time_stamp();
    }else if(window.dbms=='mysql'){
        url="template/mysql/mysql_tables.html?tmp="+time_stamp();
    }else if(window.dbms=='sqlserver'){
        url="template/sqlserver/sqlserver_tables.html?tmp="+time_stamp();
    }else if(window.dbms=='postgresql'){
        url="template/postgresql/postgresql_tables.html?tmp="+time_stamp();
    }
    $.get(url, function(temp){
        var json=JSON.parse(localStorage.getItem(struct_data_storage_key+"tables"));
        var form={'tableName':''};
        var html = template.render(temp, {'data': json.data,'lang':lang,'form':form});
        $("#container").html(html);
    });
}

//加载结构页面
function generation_html(){
    var url="";
    if(window.dbms=='oracle'){
        url="template/oracle/oracle_generation.html?tmp="+time_stamp();
    }else if(window.dbms=='mysql'){
        url="template/mysql/mysql_generation.html?tmp="+time_stamp();
    }else if(window.dbms=='sqlserver'){
        url="template/sqlserver/sqlserver_generation.html?tmp="+time_stamp();
    }else if(window.dbms=='postgresql'){
        url="template/postgresql/postgresql_generation.html?tmp="+time_stamp();
    }
    $.get(url, function(temp){
        var json=JSON.parse(localStorage.getItem(struct_data_storage_key+"generation"));
        var formData={'dbms':window.dbms,'host':window.host,'port':window.port,'dbname':window.dbname,"user":user,"pass":pass,'tb':window.tb};
        var form={'tableName':''};
        var html = template.render(temp, {'data': json.data,'formData':formData,'lang':lang,'form':form});
        $("#container").html(html);
    });
}

//得到表数据结构
function get_generation(){
    //后台处理的方法
    var method='generation';
    //所有选中的表
    window.tb="";
    var ck=$("#table tbody :checked");
    if(ck.length==0){
        alert(lang.tables_form.table_required);
        return false;
    }
    for(var i=0;i<ck.length;i++){
        tb+="'"+ck[i].value+"',";
    }
    tb=tb.substr(0,tb.length-1);
    localStorage.setItem(struct_data_storage_key+"tb",tb);
    //ajax提交
    var data={"dbms":window.dbms,"host":window.host,"port":window.port,"dbname":window.dbname,"user":window.user,"pass":window.pass,"method":method,"tb":window.tb};
    var url="php/fun.php";
    var ajaxOpt={
        data:data,
        success:function(json){
            localStorage.setItem(struct_data_storage_key+"generation",JSON.stringify(json));
            if(json.success){
                location.href='#generation';
            }else{
                alert(json.msg);
            }
        }
    };
    jqueryAjax(url,ajaxOpt);
}

//单选全选/反选功能
function checkAllOpt(){
    var ck=document.getElementById("checkAll").checked;
    $("#table :checkbox").each(function(index, domEle){
        domEle.checked=ck;
    });
}

//单个选中，控制最后全选
function checkOpt(optEle){
    var len=$("#table tbody :checkbox").length;
    var ck_len=$("#table tbody :checked").length;
    if(ck_len==len && len>0){
        document.getElementById("checkAll").checked=true;
    }else{
        document.getElementById("checkAll").checked=false;
    }
}

//localStorage缓存的前缀
var struct_data_storage_key="database_struct_storage_";
//设置数据库信息
function set_database_var(){
    localStorage.setItem(struct_data_storage_key+"dbms",dbms);
    localStorage.setItem(struct_data_storage_key+"host",host);
    localStorage.setItem(struct_data_storage_key+"port",port);
    localStorage.setItem(struct_data_storage_key+"dbname",dbname);
    localStorage.setItem(struct_data_storage_key+"user",user);
    localStorage.setItem(struct_data_storage_key+"pass",pass);
}
//获取数据库信息
function get_database_var(){
    window.dbms=localStorage.getItem(struct_data_storage_key+"dbms");
    window.host=localStorage.getItem(struct_data_storage_key+"host");
    window.port=localStorage.getItem(struct_data_storage_key+"port");
    window.dbname=localStorage.getItem(struct_data_storage_key+"dbname");
    window.user=localStorage.getItem(struct_data_storage_key+"user");
    window.pass=localStorage.getItem(struct_data_storage_key+"pass");
    window.tb=localStorage.getItem(struct_data_storage_key+"tb");
}

//创建时间戳
function time_stamp(){
    return new Date().getTime();
}

//支持的语言列表
var i18ns=['en-us','zh-cn'];

//支持的语言数组
var lang_arr=[
    {"key":"zh-cn","val":"简体中文"},
    {"key":"en-us","val":"English"}
];

//支持的数据库
var support_database=[
    'oracle','mysql','sqlserver','postgresql'
];