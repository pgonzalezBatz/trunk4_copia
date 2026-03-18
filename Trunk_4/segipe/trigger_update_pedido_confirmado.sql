CREATE OR REPLACE TRIGGER set_pedido_confirmado
  AFTER UPDATE OF id_estado ON gclinped 
    DECLARE
      n_no_aceptados number;
    BEGIN
      select count(*) into n_no_aceptados from gclinped g1 where g1.numpedlin=:new.numpedlin and g1.id_estado<>5;
      if  n_no_aceptados > 0 then 
	update gccabped set confirmado=1 where numpedcab=:new.numpedlin;
        commit;
      END IF;
    END;
/

drop trigger set_pedido_confirmado
