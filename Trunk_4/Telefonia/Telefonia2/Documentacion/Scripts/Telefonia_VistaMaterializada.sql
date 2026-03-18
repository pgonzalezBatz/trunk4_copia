CREATE MATERIALIZED VIEW "TELEFONIA"."W_FACTURA_MOVIL" ("ID_USUARIO", "NOMBRE", "APELLIDO1", "APELLIDO2", "TELEFONO", "EXTENSION", "TRAFICO", "TIPO_LLAMADA", "TIPO_DESTINO", "NUMERO_LLAMADO", "FECHA", "HORA", "TIEMPO", "IMPORTE", "ID_TLFNO", "ID_EXTENSION", "ID", "FECHA_INSERCION", "PERFIL", "TOPE") ORGANIZATION HEAP PCTFREE 10 PCTUSED 0 INITRANS 2 MAXTRANS 255 NOCOMPRESS LOGGING STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645 PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT) TABLESPACE "TELEFONIA" BUILD IMMEDIATE USING INDEX REFRESH COMPLETE ON DEMAND USING DEFAULT LOCAL ROLLBACK SEGMENT USING ENFORCED CONSTRAINTS DISABLE QUERY REWRITE
AS
  SELECT ep.id_usuario,
    u.nombre,
    u.apellido1,
    u.apellido2,
    f.*,
    pm.nombre AS perfil,
    pm.tope
    --Caso normal: Extension movil con extension interna relacionada
  FROM facturas_moviles f
  INNER JOIN extension em
  ON f.extension         = em.extension
  AND em.id_ext_interna IS NOT NULL
  INNER JOIN extension ei
  ON em.id_ext_interna = ei.id
  INNER JOIN extension_personas ep
  ON ei.id = ep.id_extension
  INNER JOIN sab.usuarios u
  ON u.id = ep.id_usuario
  INNER JOIN telefono t
  ON f.telefono = t.numero
  INNER JOIN perfiles_movil pm
  ON t.id_perfil_mov   = pm.id
  AND pm.id_planta     = 1
  WHERE ( (ep.f_desde <= f.fecha)
  AND ( (ep.f_hasta   IS NULL)
  OR (ep.f_hasta      IS NOT NULL
  AND ep.f_hasta      >= f.fecha)))
  UNION
  --Caso raro 1: Extensiones moviles que no estan asociadas a ninguna persona pero en su dia, estuvieron relacionada con una extension interna
  SELECT 0 id_usuario,
    o.nombre,
    ' ' apellido1,
    ' ' apellido2,
    f.*,
    pm.nombre AS perfil,
    pm.tope
  FROM facturas_moviles f
  INNER JOIN extension em
  ON f.extension         = em.extension
  AND em.id_ext_interna IS NULL
  INNER JOIN telefono t
  ON f.telefono = t.numero
  INNER JOIN perfiles_movil pm
  ON t.id_perfil_mov = pm.id
  AND pm.id_planta   = 1
  LEFT JOIN extension_otros eo
  ON em.id = eo.id_extension
  LEFT JOIN otros o
  ON eo.id_otro = o.id
  WHERE em.ID NOT IN
    (SELECT EP.ID_EXTENSION FROM EXTENSION_PERSONAS EP
    )
  UNION
  --Caso normal: Extension moviles libres y extensiones <5900 asociadas directamente a una persona desde su creacion (sin extension interna asociada)
  SELECT ep.id_usuario,
    u.nombre,
    u.apellido1,
    u.apellido2,
    f.*,
    pm.nombre AS perfil,
    pm.tope
  FROM facturas_moviles f
  INNER JOIN extension em
  ON f.extension         = em.extension
  AND em.id_ext_interna IS NULL 
  --AND em.id_tipoasig     = 0
  INNER JOIN extension_personas ep
  ON em.id = ep.id_extension
  INNER JOIN sab.usuarios u
  ON u.id = ep.id_usuario
  INNER JOIN telefono t
  ON f.telefono = t.numero
  INNER JOIN perfiles_movil pm
  ON t.id_perfil_mov   = pm.id
  AND pm.id_planta     = 1
  WHERE ( (ep.f_desde <= f.fecha)
  AND ( (ep.f_hasta   IS NULL)
  OR (ep.f_hasta      IS NOT NULL
  AND ep.f_hasta      >= f.fecha)))
  UNION
  --Dispositivos moviles sin extension movil asociada. Ejemplo. Pendrives de internet...
  SELECT 0 id_usuario,
    ' ' nombre,
    ' ' apellido1,
    ' ' apellido2,
    f.*,
    pm.nombre AS perfil,
    pm.tope
  FROM facturas_moviles f
  INNER JOIN telefono t
  ON f.telefono = t.numero
  INNER JOIN perfiles_movil pm
  ON t.id_perfil_mov = pm.id
  AND pm.id_planta   = 1
  WHERE extension   IS NULL;