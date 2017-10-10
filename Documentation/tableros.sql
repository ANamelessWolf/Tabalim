CREATE TABLE "tableros" (
  "tab_id" INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE,
  "prj_id" INTEGER NOT NULL,
  "tab_name" VARCHAR, 
  "tab_desc" TEXT,
  "sys_index" INTEGER, 
  "is_interruptor" BOOL, 
  "polos" INTEGER, 
  "temperature" INTEGER, 
  FOREIGN KEY(prj_id) REFERENCES proyectos(prj_id) ON DELETE CASCADE);