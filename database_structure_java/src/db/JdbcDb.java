package db;

import java.lang.reflect.Method;
import java.math.BigDecimal;
import java.sql.*;
import java.util.*;

import vo.*;

/**
 * 抽离出来的jdbc公共类
 *
 */
public class JdbcDb {
	
	/**
	 * 默认的连接，jdbc类需要自己实现
	 * @param postParam
	 * @return
	 * @throws SQLException
	 */
	public static Connection getConnection(PostParamVo postParam) throws SQLException {
		return null;
	}

	/** select 执行sql 返回单行单列数据  需要手动关闭连接
	 * @param sql
	 * @param parameters
	 * @param connection
	 * @return Object 需要手动转换类型
	 */
	public static Object executeQueryOneColumn(String sql, String[] parameters,Connection connection) {
		try (PreparedStatement ps = connection.prepareStatement(sql)){
			if (parameters != null) {
				for (int i = 0; i < parameters.length; i++) {
					ps.setString(i + 1, parameters[i]);
				}
			}
			ResultSet rs = ps.executeQuery();
			rs.next();
			Object ret=rs.getObject(1);
			rs.close();
			return ret;
		} catch (SQLException e) {
			e.printStackTrace();
			throw new RuntimeException(e.getMessage());
		}
	}
	
	/** select count() sum() avg() .. 返回单行单列数据
	 * @param sql
	 * @param parameters
	 * @param connection
	 * @return
	 * @throws Exception 
	 */
	public static <T> ArrayList<T> executeQuery(String sql, String[] parameters,Connection connection,Class<T> obj) throws Exception{
		PreparedStatement ps = connection.prepareStatement(sql);
		if (parameters != null) {
			for (int i = 0; i < parameters.length; i++) {
				ps.setString(i + 1, parameters[i]);
			}
		}
		ResultSet rs = ps.executeQuery();
		ResultSetMetaData metaData = rs.getMetaData();
		ArrayList<T> arrayList = new ArrayList<T>();
        //获取总列数
        int count = metaData.getColumnCount();
        while (rs.next()) {
            //创建对象实例
            T newInstance = obj.newInstance();
            for (int i = 1; i <= count; i++) {
                //给对象的某个属性赋值
                String name = metaData.getColumnLabel(i);
                // 首字母大写
                String substring = name.substring(0, 1);
                String replace = name.replaceFirst(substring, substring.toUpperCase());
                Class<?> type = null;
                try {
                    type = obj.getDeclaredField(name).getType();
                } catch (NoSuchFieldException e) { // Class对象未定义该字段时,跳过
                    continue;
                }
                Method method = obj.getMethod("set" + replace, type);
                //判断读取数据的类型
                if (type.isAssignableFrom(String.class)) {
                	//String
                    method.invoke(newInstance, rs.getString(i));
                } else if (type.isAssignableFrom(byte.class) || type.isAssignableFrom(Byte.class)) {
                	// byte 数据类型是8位、有符号的，以二进制补码表示的整数
                    method.invoke(newInstance, rs.getByte(i));
                } else if (type.isAssignableFrom(short.class) || type.isAssignableFrom(Short.class)) {
                	// short 数据类型是 16 位、有符号的以二进制补码表示的整数
                    method.invoke(newInstance, rs.getShort(i));
                } else if (type.isAssignableFrom(int.class) || type.isAssignableFrom(Integer.class)) {
                	// int 数据类型是32位、有符号的以二进制补码表示的整数
                    method.invoke(newInstance, rs.getInt(i));
                } else if (type.isAssignableFrom(long.class) || type.isAssignableFrom(Long.class)) {
                	// long 数据类型是 64 位、有符号的以二进制补码表示的整数
                    method.invoke(newInstance, rs.getLong(i));
                } else if (type.isAssignableFrom(float.class) || type.isAssignableFrom(Float.class)) {
                	// float 数据类型是单精度、32位、符合IEEE 754标准的浮点数
                    method.invoke(newInstance, rs.getFloat(i));
                } else if (type.isAssignableFrom(double.class) || type.isAssignableFrom(Double.class)) {
                	// double 数据类型是双精度、64 位、符合IEEE 754标准的浮点数
                    method.invoke(newInstance, rs.getDouble(i));
                } else if (type.isAssignableFrom(BigDecimal.class)) {
                    method.invoke(newInstance, rs.getBigDecimal(i));
                } else if (type.isAssignableFrom(boolean.class) || type.isAssignableFrom(Boolean.class)) {
                	// boolean数据类型表示一位的信息
                    method.invoke(newInstance, rs.getBoolean(i));
                } else if (type.isAssignableFrom(java.util.Date.class)) {
                    method.invoke(newInstance, rs.getDate(i));
                }
            }
            arrayList.add(newInstance);
        }
        rs.close();
        ps.close();
        return arrayList;
	}
}
