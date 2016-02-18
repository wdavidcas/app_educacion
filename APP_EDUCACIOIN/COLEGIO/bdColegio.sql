SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

CREATE SCHEMA IF NOT EXISTS `bdcolegio` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci ;
USE `bdcolegio` ;

-- -----------------------------------------------------
-- Table `bdcolegio`.`Edificio`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`Edificio` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`Edificio` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `nombre` VARCHAR(25) NULL ,
  `descripcion` VARCHAR(100) NULL ,
  `estado` TINYINT(1) NULL ,
  PRIMARY KEY (`id`) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `bdcolegio`.`Modulo`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`Modulo` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`Modulo` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `nombre` VARCHAR(25) NULL ,
  `descripcion` VARCHAR(100) NULL ,
  `estado` TINYINT(1) NULL ,
  `Edificio_id` INT NOT NULL ,
  PRIMARY KEY (`id`) ,
  CONSTRAINT `fk_Modulo_Edificio1`
    FOREIGN KEY (`Edificio_id` )
    REFERENCES `bdcolegio`.`Edificio` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_Modulo_Edificio1_idx` ON `bdcolegio`.`Modulo` (`Edificio_id` ASC) ;


-- -----------------------------------------------------
-- Table `bdcolegio`.`Aula`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`Aula` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`Aula` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `nombre` VARCHAR(25) NULL ,
  `descripcion` VARCHAR(100) NULL ,
  `estado` TINYINT(1) NULL ,
  `Modulo_id` INT NOT NULL ,
  PRIMARY KEY (`id`) ,
  CONSTRAINT `fk_Aula_Modulo1`
    FOREIGN KEY (`Modulo_id` )
    REFERENCES `bdcolegio`.`Modulo` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_Aula_Modulo1_idx` ON `bdcolegio`.`Aula` (`Modulo_id` ASC) ;


-- -----------------------------------------------------
-- Table `bdcolegio`.`Alumno`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`Alumno` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`Alumno` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `nombre` VARCHAR(25) NULL ,
  `apellido` VARCHAR(25) NULL ,
  `direccion` VARCHAR(100) NULL ,
  `carnet` VARCHAR(10) NULL ,
  `correo` VARCHAR(50) NULL ,
  `fechaNacimiento` DATETIME NULL ,
  `fotografia` VARCHAR(45) NULL ,
  `cui` VARCHAR(45) NULL ,
  `estado` INT NULL ,
  PRIMARY KEY (`id`) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `bdcolegio`.`AsignacionCurso`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`AsignacionCurso` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`AsignacionCurso` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `fecha` DATE NULL ,
  `notaFinal` FLOAT NULL ,
  `Alumno_id` INT NOT NULL ,
  `estado` TINYINT(1) NULL ,
  PRIMARY KEY (`id`) ,
  CONSTRAINT `fk_AsignacionCurso_Alumno1`
    FOREIGN KEY (`Alumno_id` )
    REFERENCES `bdcolegio`.`Alumno` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_AsignacionCurso_Alumno1_idx` ON `bdcolegio`.`AsignacionCurso` (`Alumno_id` ASC) ;


-- -----------------------------------------------------
-- Table `bdcolegio`.`AsignacionAula`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`AsignacionAula` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`AsignacionAula` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `Aula_id` INT NOT NULL ,
  `AsignacionCurso_id` INT NOT NULL ,
  `estado` TINYINT(1) NULL ,
  PRIMARY KEY (`id`) ,
  CONSTRAINT `fk_AsignacionAula_Aula1`
    FOREIGN KEY (`Aula_id` )
    REFERENCES `bdcolegio`.`Aula` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_AsignacionAula_AsignacionCurso1`
    FOREIGN KEY (`AsignacionCurso_id` )
    REFERENCES `bdcolegio`.`AsignacionCurso` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_AsignacionAula_Aula1_idx` ON `bdcolegio`.`AsignacionAula` (`Aula_id` ASC) ;

CREATE INDEX `fk_AsignacionAula_AsignacionCurso1_idx` ON `bdcolegio`.`AsignacionAula` (`AsignacionCurso_id` ASC) ;


-- -----------------------------------------------------
-- Table `bdcolegio`.`Unidad`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`Unidad` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`Unidad` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `nombre` VARCHAR(25) NULL ,
  `descripcion` VARCHAR(100) NULL ,
  `fechaInicial` DATE NULL ,
  `fechaFinal` DATE NULL ,
  `notaZona` FLOAT NULL ,
  `acreditacionExamen` FLOAT NULL ,
  `examenUnidad` FLOAT NULL ,
  `estado` TINYINT(1) NULL ,
  `AsignacionCurso_id` INT NOT NULL ,
  PRIMARY KEY (`id`) ,
  CONSTRAINT `fk_Unidad_AsignacionCurso1`
    FOREIGN KEY (`AsignacionCurso_id` )
    REFERENCES `bdcolegio`.`AsignacionCurso` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_Unidad_AsignacionCurso1_idx` ON `bdcolegio`.`Unidad` (`AsignacionCurso_id` ASC) ;


-- -----------------------------------------------------
-- Table `bdcolegio`.`Actividad`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`Actividad` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`Actividad` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `nombre` VARCHAR(50) NULL ,
  `descripcion` VARCHAR(150) NULL ,
  `fecha` DATE NULL ,
  `puntosAsignados` FLOAT NULL ,
  `puntosGanados` FLOAT NULL ,
  `estado` TINYINT(1) NULL ,
  `Unidad_id` INT NOT NULL ,
  PRIMARY KEY (`id`) ,
  CONSTRAINT `fk_Actividad_Unidad1`
    FOREIGN KEY (`Unidad_id` )
    REFERENCES `bdcolegio`.`Unidad` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_Actividad_Unidad1_idx` ON `bdcolegio`.`Actividad` (`Unidad_id` ASC) ;


-- -----------------------------------------------------
-- Table `bdcolegio`.`CategoriaNivel`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`CategoriaNivel` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`CategoriaNivel` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `nombre` VARCHAR(25) NULL ,
  `descripcion` VARCHAR(100) NULL ,
  `estado` TINYINT(1) NULL DEFAULT TRUE ,
  PRIMARY KEY (`id`) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `bdcolegio`.`Nivel`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`Nivel` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`Nivel` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `nombre` VARCHAR(25) NULL ,
  `descripcion` VARCHAR(100) NULL ,
  `estado` TINYINT(1) NULL ,
  `CategoriaNivel_id` INT NOT NULL ,
  PRIMARY KEY (`id`) ,
  CONSTRAINT `fk_Nivel_CategoriaNivel1`
    FOREIGN KEY (`CategoriaNivel_id` )
    REFERENCES `bdcolegio`.`CategoriaNivel` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_Nivel_CategoriaNivel1_idx` ON `bdcolegio`.`Nivel` (`CategoriaNivel_id` ASC) ;


-- -----------------------------------------------------
-- Table `bdcolegio`.`AsignacionNivel`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`AsignacionNivel` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`AsignacionNivel` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `fecha` DATE NULL ,
  `Nivel_id` INT NOT NULL ,
  `Alumno_id` INT NOT NULL ,
  PRIMARY KEY (`id`) ,
  CONSTRAINT `fk_AsignacionNivel_Nivel1`
    FOREIGN KEY (`Nivel_id` )
    REFERENCES `bdcolegio`.`Nivel` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_AsignacionNivel_Alumno1`
    FOREIGN KEY (`Alumno_id` )
    REFERENCES `bdcolegio`.`Alumno` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_AsignacionNivel_Nivel1_idx` ON `bdcolegio`.`AsignacionNivel` (`Nivel_id` ASC) ;

CREATE INDEX `fk_AsignacionNivel_Alumno1_idx` ON `bdcolegio`.`AsignacionNivel` (`Alumno_id` ASC) ;


-- -----------------------------------------------------
-- Table `bdcolegio`.`TipoEstado`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`TipoEstado` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`TipoEstado` (
  `idTipoEstado` INT NOT NULL AUTO_INCREMENT ,
  `nombre` VARCHAR(45) NULL ,
  `descripcion` VARCHAR(45) NULL ,
  `habilitado` BLOB NULL ,
  PRIMARY KEY (`idTipoEstado`) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `bdcolegio`.`Estado`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`Estado` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`Estado` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `nombre` VARCHAR(25) NULL ,
  `descripcion` VARCHAR(100) NULL ,
  `estado` TINYINT(1) NULL ,
  `TipoEstado_idTipoEstado` INT NOT NULL ,
  PRIMARY KEY (`id`) ,
  CONSTRAINT `fk_Estado_TipoEstado1`
    FOREIGN KEY (`TipoEstado_idTipoEstado` )
    REFERENCES `bdcolegio`.`TipoEstado` (`idTipoEstado` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_Estado_TipoEstado1_idx` ON `bdcolegio`.`Estado` (`TipoEstado_idTipoEstado` ASC) ;


-- -----------------------------------------------------
-- Table `bdcolegio`.`Parentesco`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`Parentesco` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`Parentesco` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `nombre` VARCHAR(25) NULL ,
  `descripcion` VARCHAR(100) NULL ,
  `estado` TINYINT(1) NULL ,
  PRIMARY KEY (`id`) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `bdcolegio`.`Familiares`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`Familiares` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`Familiares` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `nombre` VARCHAR(25) NULL ,
  `apellido` VARCHAR(25) NULL ,
  `direccion` VARCHAR(100) NULL ,
  `correo` VARCHAR(100) NULL ,
  `Parentesco_id` INT NOT NULL ,
  `Alumno_id` INT NOT NULL ,
  `esResponsableDirecto` BIT NULL ,
  `nivelResponsabilidad` INT NULL ,
  `Estado_id` INT NOT NULL ,
  PRIMARY KEY (`id`) ,
  CONSTRAINT `fk_Responsable_Parentesco1`
    FOREIGN KEY (`Parentesco_id` )
    REFERENCES `bdcolegio`.`Parentesco` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Responsable_Alumno1`
    FOREIGN KEY (`Alumno_id` )
    REFERENCES `bdcolegio`.`Alumno` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Familiares_Estado1`
    FOREIGN KEY (`Estado_id` )
    REFERENCES `bdcolegio`.`Estado` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_Responsable_Parentesco1_idx` ON `bdcolegio`.`Familiares` (`Parentesco_id` ASC) ;

CREATE INDEX `fk_Responsable_Alumno1_idx` ON `bdcolegio`.`Familiares` (`Alumno_id` ASC) ;

CREATE INDEX `fk_Familiares_Estado1_idx` ON `bdcolegio`.`Familiares` (`Estado_id` ASC) ;


-- -----------------------------------------------------
-- Table `bdcolegio`.`TipoTelefono`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`TipoTelefono` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`TipoTelefono` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `nombre` VARCHAR(25) NULL ,
  `descripcion` VARCHAR(100) NULL ,
  `estado` TINYINT(1) NULL ,
  PRIMARY KEY (`id`) )
ENGINE = InnoDB;

CREATE UNIQUE INDEX `id_UNIQUE` ON `bdcolegio`.`TipoTelefono` (`id` ASC) ;


-- -----------------------------------------------------
-- Table `bdcolegio`.`PROFESION`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`PROFESION` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`PROFESION` (
  `idPROFESION` INT NOT NULL ,
  `nombre` VARCHAR(45) NULL ,
  `descripcion` VARCHAR(45) NULL ,
  `estado` TINYINT(1) NULL DEFAULT 1 ,
  PRIMARY KEY (`idPROFESION`) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `bdcolegio`.`Profesor`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`Profesor` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`Profesor` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `nombre` VARCHAR(25) NULL ,
  `apellido` VARCHAR(25) NULL ,
  `direccion` VARCHAR(100) NULL ,
  `correo` VARCHAR(50) NULL ,
  `estudia` TINYINT(1) NULL ,
  `fechaNacimiento` DATE NULL ,
  `cui` VARCHAR(45) NULL ,
  `Estado_id` INT NOT NULL ,
  `PROFESION_idPROFESION` INT NOT NULL ,
  PRIMARY KEY (`id`) ,
  CONSTRAINT `fk_Profesor_Estado1`
    FOREIGN KEY (`Estado_id` )
    REFERENCES `bdcolegio`.`Estado` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Profesor_PROFESION1`
    FOREIGN KEY (`PROFESION_idPROFESION` )
    REFERENCES `bdcolegio`.`PROFESION` (`idPROFESION` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_Profesor_Estado1_idx` ON `bdcolegio`.`Profesor` (`Estado_id` ASC) ;

CREATE INDEX `fk_Profesor_PROFESION1_idx` ON `bdcolegio`.`Profesor` (`PROFESION_idPROFESION` ASC) ;


-- -----------------------------------------------------
-- Table `bdcolegio`.`Telefono`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`Telefono` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`Telefono` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `numero` VARCHAR(11) NULL ,
  `estado` TINYINT(1) NULL ,
  `TipoTelefono_id` INT NOT NULL ,
  `Alumno_id` INT NULL ,
  `Responsable_id` INT NULL ,
  `Profesor_id` INT NULL ,
  PRIMARY KEY (`id`) ,
  CONSTRAINT `fk_Telefono_TipoTelefono1`
    FOREIGN KEY (`TipoTelefono_id` )
    REFERENCES `bdcolegio`.`TipoTelefono` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Telefono_Alumno1`
    FOREIGN KEY (`Alumno_id` )
    REFERENCES `bdcolegio`.`Alumno` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Telefono_Responsable1`
    FOREIGN KEY (`Responsable_id` )
    REFERENCES `bdcolegio`.`Familiares` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Telefono_Profesor1`
    FOREIGN KEY (`Profesor_id` )
    REFERENCES `bdcolegio`.`Profesor` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_Telefono_TipoTelefono1_idx` ON `bdcolegio`.`Telefono` (`TipoTelefono_id` ASC) ;

CREATE INDEX `fk_Telefono_Alumno1_idx` ON `bdcolegio`.`Telefono` (`Alumno_id` ASC) ;

CREATE INDEX `fk_Telefono_Responsable1_idx` ON `bdcolegio`.`Telefono` (`Responsable_id` ASC) ;

CREATE INDEX `fk_Telefono_Profesor1_idx` ON `bdcolegio`.`Telefono` (`Profesor_id` ASC) ;


-- -----------------------------------------------------
-- Table `bdcolegio`.`Categoria`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`Categoria` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`Categoria` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `nombre` VARCHAR(25) NULL ,
  `descripcion` VARCHAR(100) NULL ,
  `estado` TINYINT(1) NULL ,
  PRIMARY KEY (`id`) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `bdcolegio`.`Curso`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`Curso` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`Curso` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `nombre` VARCHAR(25) NULL ,
  `descripcion` VARCHAR(100) NULL ,
  `creditos` INT(11) NULL ,
  `Categoria_id` INT NOT NULL ,
  `Estado_id` INT NOT NULL ,
  PRIMARY KEY (`id`) ,
  CONSTRAINT `fk_Curso_Categoria1`
    FOREIGN KEY (`Categoria_id` )
    REFERENCES `bdcolegio`.`Categoria` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Curso_Estado1`
    FOREIGN KEY (`Estado_id` )
    REFERENCES `bdcolegio`.`Estado` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_Curso_Categoria1_idx` ON `bdcolegio`.`Curso` (`Categoria_id` ASC) ;

CREATE INDEX `fk_Curso_Estado1_idx` ON `bdcolegio`.`Curso` (`Estado_id` ASC) ;


-- -----------------------------------------------------
-- Table `bdcolegio`.`AsignacionCatedra`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`AsignacionCatedra` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`AsignacionCatedra` (
  `idAsignacionCatedra` INT NOT NULL AUTO_INCREMENT ,
  `Profesor_id` INT NOT NULL ,
  `Curso_id` INT NOT NULL ,
  `estado` TINYINT(1) NULL ,
  PRIMARY KEY (`idAsignacionCatedra`) ,
  CONSTRAINT `fk_AsignacionCatedra_Profesor1`
    FOREIGN KEY (`Profesor_id` )
    REFERENCES `bdcolegio`.`Profesor` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_AsignacionCatedra_Curso1`
    FOREIGN KEY (`Curso_id` )
    REFERENCES `bdcolegio`.`Curso` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_AsignacionCatedra_Profesor1_idx` ON `bdcolegio`.`AsignacionCatedra` (`Profesor_id` ASC) ;

CREATE INDEX `fk_AsignacionCatedra_Curso1_idx` ON `bdcolegio`.`AsignacionCatedra` (`Curso_id` ASC) ;


-- -----------------------------------------------------
-- Table `bdcolegio`.`DiaSemana`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`DiaSemana` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`DiaSemana` (
  `idDiaSemana` INT NOT NULL AUTO_INCREMENT ,
  `nombre` VARCHAR(45) NULL ,
  `Estado_id` INT NOT NULL ,
  PRIMARY KEY (`idDiaSemana`) ,
  CONSTRAINT `fk_DiaSemana_Estado1`
    FOREIGN KEY (`Estado_id` )
    REFERENCES `bdcolegio`.`Estado` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_DiaSemana_Estado1_idx` ON `bdcolegio`.`DiaSemana` (`Estado_id` ASC) ;


-- -----------------------------------------------------
-- Table `bdcolegio`.`PeriodosClase`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`PeriodosClase` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`PeriodosClase` (
  `idPeriodosClase` INT NOT NULL AUTO_INCREMENT ,
  `horaInicio` VARCHAR(45) NULL ,
  `horaFinal` VARCHAR(45) NULL ,
  `DiaSemana_idDiaSemana` INT NOT NULL ,
  `AsignacionCatedra_idAsignacionCatedra` INT NOT NULL ,
  PRIMARY KEY (`idPeriodosClase`) ,
  CONSTRAINT `fk_PeriodosClase_DiaSemana1`
    FOREIGN KEY (`DiaSemana_idDiaSemana` )
    REFERENCES `bdcolegio`.`DiaSemana` (`idDiaSemana` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_PeriodosClase_AsignacionCatedra1`
    FOREIGN KEY (`AsignacionCatedra_idAsignacionCatedra` )
    REFERENCES `bdcolegio`.`AsignacionCatedra` (`idAsignacionCatedra` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_PeriodosClase_DiaSemana1_idx` ON `bdcolegio`.`PeriodosClase` (`DiaSemana_idDiaSemana` ASC) ;

CREATE INDEX `fk_PeriodosClase_AsignacionCatedra1_idx` ON `bdcolegio`.`PeriodosClase` (`AsignacionCatedra_idAsignacionCatedra` ASC) ;


-- -----------------------------------------------------
-- Table `bdcolegio`.`INVENTARIO_FISICO`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`INVENTARIO_FISICO` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`INVENTARIO_FISICO` (
  `id` INT NOT NULL AUTO_INCREMENT ,
  `nombre` VARCHAR(45) NULL ,
  `descripcion` VARCHAR(45) NULL ,
  `existencias` INT NULL ,
  `precio` FLOAT NULL ,
  `depreciacion` FLOAT NULL ,
  `marca` VARCHAR(45) NULL ,
  `Aula_id` INT NULL ,
  `modelo` VARCHAR(45) NULL ,
  `Modulo_id` INT NULL ,
  `Edificio_id` INT NULL ,
  `Estado_id` INT NOT NULL ,
  PRIMARY KEY (`id`) ,
  CONSTRAINT `fk_COSAS_Aula1`
    FOREIGN KEY (`Aula_id` )
    REFERENCES `bdcolegio`.`Aula` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_INVENTARIO_FISICO_Modulo1`
    FOREIGN KEY (`Modulo_id` )
    REFERENCES `bdcolegio`.`Modulo` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_INVENTARIO_FISICO_Edificio1`
    FOREIGN KEY (`Edificio_id` )
    REFERENCES `bdcolegio`.`Edificio` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_INVENTARIO_FISICO_Estado1`
    FOREIGN KEY (`Estado_id` )
    REFERENCES `bdcolegio`.`Estado` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_COSAS_Aula1_idx` ON `bdcolegio`.`INVENTARIO_FISICO` (`Aula_id` ASC) ;

CREATE INDEX `fk_INVENTARIO_FISICO_Modulo1_idx` ON `bdcolegio`.`INVENTARIO_FISICO` (`Modulo_id` ASC) ;

CREATE INDEX `fk_INVENTARIO_FISICO_Edificio1_idx` ON `bdcolegio`.`INVENTARIO_FISICO` (`Edificio_id` ASC) ;

CREATE INDEX `fk_INVENTARIO_FISICO_Estado1_idx` ON `bdcolegio`.`INVENTARIO_FISICO` (`Estado_id` ASC) ;


-- -----------------------------------------------------
-- Table `bdcolegio`.`CURSO_NIVEL`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `bdcolegio`.`CURSO_NIVEL` ;

CREATE  TABLE IF NOT EXISTS `bdcolegio`.`CURSO_NIVEL` (
  `Nivel_id` INT NOT NULL ,
  `Curso_id` INT NOT NULL ,
  `fechaAsignacion` DATETIME NULL ,
  `Estado` INT NULL ,
  `Creditos` INT NULL ,
  `CodigoMineduc` VARCHAR(45) NULL ,
  `CodigoInterno` VARCHAR(45) NULL ,
  `NotaMinomoAprobacion` FLOAT NULL ,
  PRIMARY KEY (`Nivel_id`, `Curso_id`) ,
  CONSTRAINT `fk_Nivel_has_Curso_Nivel1`
    FOREIGN KEY (`Nivel_id` )
    REFERENCES `bdcolegio`.`Nivel` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Nivel_has_Curso_Curso1`
    FOREIGN KEY (`Curso_id` )
    REFERENCES `bdcolegio`.`Curso` (`id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_Nivel_has_Curso_Curso1_idx` ON `bdcolegio`.`CURSO_NIVEL` (`Curso_id` ASC) ;

CREATE INDEX `fk_Nivel_has_Curso_Nivel1_idx` ON `bdcolegio`.`CURSO_NIVEL` (`Nivel_id` ASC) ;



SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

-- -----------------------------------------------------
-- Data for table `bdcolegio`.`CategoriaNivel`
-- -----------------------------------------------------
START TRANSACTION;
USE `bdcolegio`;
INSERT INTO `bdcolegio`.`CategoriaNivel` (`id`, `nombre`, `descripcion`, `estado`) VALUES (1, 'PRIMARIA', 'PRIMARIA', 1);
INSERT INTO `bdcolegio`.`CategoriaNivel` (`id`, `nombre`, `descripcion`, `estado`) VALUES (2, 'BASICO', 'BASICO', 1);
INSERT INTO `bdcolegio`.`CategoriaNivel` (`id`, `nombre`, `descripcion`, `estado`) VALUES (3, 'DIVERSIFICADO', 'DIVERSIFICADO', 1);
INSERT INTO `bdcolegio`.`CategoriaNivel` (`id`, `nombre`, `descripcion`, `estado`) VALUES (4, 'LICENCIATURAS', 'LICENCIATURAS', 1);

COMMIT;

-- -----------------------------------------------------
-- Data for table `bdcolegio`.`Nivel`
-- -----------------------------------------------------
START TRANSACTION;
USE `bdcolegio`;
INSERT INTO `bdcolegio`.`Nivel` (`id`, `nombre`, `descripcion`, `estado`, `CategoriaNivel_id`) VALUES (1, 'PRIMERO', 'PRIMERO', 1, 1);
INSERT INTO `bdcolegio`.`Nivel` (`id`, `nombre`, `descripcion`, `estado`, `CategoriaNivel_id`) VALUES (2, 'SEGUNDO', 'SEGUNDO', 1, 1);
INSERT INTO `bdcolegio`.`Nivel` (`id`, `nombre`, `descripcion`, `estado`, `CategoriaNivel_id`) VALUES (3, 'TERCERO', 'TERCERO', 1, 1);
INSERT INTO `bdcolegio`.`Nivel` (`id`, `nombre`, `descripcion`, `estado`, `CategoriaNivel_id`) VALUES (4, 'CUARTO', 'CUARTO', 1, 1);
INSERT INTO `bdcolegio`.`Nivel` (`id`, `nombre`, `descripcion`, `estado`, `CategoriaNivel_id`) VALUES (5, 'QUINTO', 'QUINTO', 1, 1);
INSERT INTO `bdcolegio`.`Nivel` (`id`, `nombre`, `descripcion`, `estado`, `CategoriaNivel_id`) VALUES (6, 'SEXTO', 'SEXTO', 1, 1);

COMMIT;

-- -----------------------------------------------------
-- Data for table `bdcolegio`.`Parentesco`
-- -----------------------------------------------------
START TRANSACTION;
USE `bdcolegio`;
INSERT INTO `bdcolegio`.`Parentesco` (`id`, `nombre`, `descripcion`, `estado`) VALUES (1, 'PADRE', 'PADRE', 1);
INSERT INTO `bdcolegio`.`Parentesco` (`id`, `nombre`, `descripcion`, `estado`) VALUES (2, 'MADRE', 'MADRE', 1);
INSERT INTO `bdcolegio`.`Parentesco` (`id`, `nombre`, `descripcion`, `estado`) VALUES (3, 'TIO', 'TIO', 1);

COMMIT;

-- -----------------------------------------------------
-- Data for table `bdcolegio`.`TipoTelefono`
-- -----------------------------------------------------
START TRANSACTION;
USE `bdcolegio`;
INSERT INTO `bdcolegio`.`TipoTelefono` (`id`, `nombre`, `descripcion`, `estado`) VALUES (1, 'CELULAR', 'CELULAR', 1);
INSERT INTO `bdcolegio`.`TipoTelefono` (`id`, `nombre`, `descripcion`, `estado`) VALUES (2, 'RESIDENCIAL', 'RESIDENCIAL', 1);
INSERT INTO `bdcolegio`.`TipoTelefono` (`id`, `nombre`, `descripcion`, `estado`) VALUES (3, 'TRABAJO', 'TRABAJO', 1);
INSERT INTO `bdcolegio`.`TipoTelefono` (`id`, `nombre`, `descripcion`, `estado`) VALUES (4, 'EMERGENCIA', 'EMERGENCIA', 1);

COMMIT;

-- -----------------------------------------------------
-- Data for table `bdcolegio`.`Categoria`
-- -----------------------------------------------------
START TRANSACTION;
USE `bdcolegio`;
INSERT INTO `bdcolegio`.`Categoria` (`id`, `nombre`, `descripcion`, `estado`) VALUES (1, 'NUMERICO', 'NUMERICO', 1);
INSERT INTO `bdcolegio`.`Categoria` (`id`, `nombre`, `descripcion`, `estado`) VALUES (2, 'IDIOMAS', 'IDIOMAS', 1);
INSERT INTO `bdcolegio`.`Categoria` (`id`, `nombre`, `descripcion`, `estado`) VALUES (3, 'LENGUAJE', 'LENGUAJE', 1);

COMMIT;
