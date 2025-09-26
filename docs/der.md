erDiagram

  USUARIO {
    int id PK
    string email
    string password
    string nombre NULL
    string apellido NULL
    string avatar NULL
    enum rol
  }

  PROPIETARIO {
    int id PK
    string dni UNI
    string nombre
    string apellido
    string telefono NULL
    string email NULL
  }

  INQUILINO {
    int id PK
    string dni UNI
    string nombre
    string apellido
    string telefono NULL
    string email NULL
  }

  TIPOINMUEBLE {
    int id PK
    string nombre
  }

  INMUEBLE {
    int id PK
    string direccion
    enum uso
    int ambientes
    decimal longitud
    decimal latitud
    decimal precio
    bool estado
    int propietario_id FK
    int tipoInmueble_id FK
  }

  CONTRATO {
    int id PK
    date fechaInicio
    date fechaFin
    decimal monto
    int inquilino_id FK
    int inmueble_id FK
    datetime creado_en NULL
    int creado_por FK NULL
    datetime terminado_en NULL
    int terminado_por FK NULL
    enum estado
  }

  PAGO {
    int id PK
    int numeroPago
    date fechaPago
    decimal monto
    int contrato_id FK
    date fechaUpdate
    datetime anulado_en NULL
    int anulado_por FK NULL
    datetime creado_en NULL
    int creado_por FK NULL
    enum estado
  }

  %% Relaciones principales
  PROPIETARIO ||--o{ INMUEBLE : posee
  TIPOINMUEBLE ||--o{ INMUEBLE : clasifica
  INQUILINO ||--o{ CONTRATO : firma
  INMUEBLE ||--o{ CONTRATO : alquilado_en
  CONTRATO ||--o{ PAGO : tiene
  USUARIO ||--o{ CONTRATO : crea_termina
  USUARIO ||--o{ PAGO : crea_anula
