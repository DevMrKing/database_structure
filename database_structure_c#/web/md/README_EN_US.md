# Database document generation tool£º

#### dal£ºDatabase operation class
#### bll£ºDatabase service class
#### ashx£ºBackground processing total entrance
#### model£ºEncapsulated entity class
#### lib£ºThird party support expansion

#### i18n£ºInternational language pack
#### md£ºTranslation instructions for different languages
#### template£ºTemplate processing files written for different databases
#### view£ºView processing
#### common.js Common js processing throughout the project
#### common.css Common css processing throughout the project
#### index.html Entrance
#### jquery-1.11.0.min.js jquery
#### template-web-4.13.2.js Template rendering engine

#### Third-party support needed£ºNewtonsoft.Json.dll,MySql.Data.dll,Npgsql.dll,Mono.Security.dll,Oracle.ManagedDataAccess.dll

#### The author writes the project using the .net4.0 web application

# Secondary development£º
#### Add new language support£º
1. Shortcode for browser support for language in the i18ns array in common.js
2. Support for language options in the lang_arr array in common.js
3. Append language pack js under i18n folder

#### Add new database support£º
1. In the common.js support_database array plus the database to be supported
2. Approve the processing background of the support database under the ashx folder
3. In ashx plus if processing to support the database
4. In common.js, table_html(), generation_html(), plus the special template path to be jumped.
5. In the new template, the template should be added with custom processing code.
