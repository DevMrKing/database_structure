# Database document generation tool��

#### dal��Database operation class
#### bll��Database service class
#### ashx��Background processing total entrance
#### model��Encapsulated entity class
#### lib��Third party support expansion

#### i18n��International language pack
#### md��Translation instructions for different languages
#### template��Template processing files written for different databases
#### view��View processing
#### common.js Common js processing throughout the project
#### common.css Common css processing throughout the project
#### index.html Entrance
#### jquery-1.11.0.min.js jquery
#### template-web-4.13.2.js Template rendering engine

#### Third-party support needed��Newtonsoft.Json.dll,MySql.Data.dll,Npgsql.dll,Mono.Security.dll,Oracle.ManagedDataAccess.dll

#### The author writes the project using the .net4.0 web application

# Secondary development��
#### Add new language support��
1. Shortcode for browser support for language in the i18ns array in common.js
2. Support for language options in the lang_arr array in common.js
3. Append language pack js under i18n folder

#### Add new database support��
1. In the common.js support_database array plus the database to be supported
2. Approve the processing background of the support database under the ashx folder
3. In ashx plus if processing to support the database
4. In common.js, table_html(), generation_html(), plus the special template path to be jumped.
5. In the new template, the template should be added with custom processing code.
