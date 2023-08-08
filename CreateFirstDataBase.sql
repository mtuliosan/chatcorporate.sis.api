-- public.audiences definition

-- Drop table

-- DROP TABLE public.audiences;

CREATE TABLE public.audiences (
	id uuid NOT NULL,
	"key" varchar(100) NULL,
	"name" varchar(100) NULL,
	CONSTRAINT audiences_pkey PRIMARY KEY (id)
);

-- public.roles definition

-- Drop table

-- DROP TABLE public.roles;

CREATE TABLE public.roles (
	id uuid NOT NULL,
	value varchar(100) NULL,
	"name" varchar(100) NULL,
	CONSTRAINT roles_pkey PRIMARY KEY (id),
	CONSTRAINT roles_value_key UNIQUE (value)
);

-- public.users definition

-- Drop table

-- DROP TABLE public.users;

CREATE TABLE public.users (
	id uuid NOT NULL,
	email varchar(100) NULL,
	"password" varchar(100) NULL,
	salt varchar(200) NULL,
	"name" varchar(100) NULL,
	resetticket uuid NULL,
	resetticketexp timestamp NULL,
	sapuser bool NULL DEFAULT false,
	groupid varchar(50) NULL,
	CONSTRAINT users_email_key UNIQUE (email),
	CONSTRAINT users_pkey PRIMARY KEY (id)
);


-- public.userroles definition

-- Drop table

-- DROP TABLE public.userroles;

CREATE TABLE public.userroles (
	id serial4 NOT NULL,
	idrole uuid NULL,
	iduser uuid NULL,
	idaudience uuid NULL,
	value varchar(100) NULL,
	CONSTRAINT userroles_idrole_iduser_idaudience_key UNIQUE (idrole, iduser, idaudience),
	CONSTRAINT userroles_pkey PRIMARY KEY (id)
);


-- public.userroles foreign keys

ALTER TABLE public.userroles ADD CONSTRAINT userroles_idaudience_fkey FOREIGN KEY (idaudience) REFERENCES public.audiences(id);
ALTER TABLE public.userroles ADD CONSTRAINT userroles_idrole_fkey FOREIGN KEY (idrole) REFERENCES public.roles(id);
ALTER TABLE public.userroles ADD CONSTRAINT userroles_iduser_fkey FOREIGN KEY (iduser) REFERENCES public.users(id);