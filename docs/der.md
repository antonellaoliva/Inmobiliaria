```mermaid
erDiagram

  USUARIO {
    int id PK
    string email
    string password
    string nombre
    string apellido
    string avatar
    enum rol
  }

  PROPIETARIO {
    int id PK
    string DNI
    string nombre
    string apellido
    string telefono
    string email
  }

  INQUILINO {
    int id PK
    string DNI
    string nombre
    string apellido
    string telefono
    string email
  }

  INMUEBLE {
    int id PK
    string direccion
    enum uso
    string tipo
    int ambientes
    decimal longitud
    decimal latitud
    decimal precio
    bool estado
    int propietario_id FK
  }

  CONTRATO {
    int id PK
    date fechaInicio
    date fechaFin
    decimal monto
    int inquilino_id FK
    int inmueble_id FK
    
  }

  PAGO {
    int id PK
    int numeroPago
    date fechaPago
    decimal monto
    int contratoId FK
    date fechaUpdate

  }

  %% Relaciones principales
  PROPIETARIO ||--o{ INMUEBLE : posee
  INQUILINO ||--o{ CONTRATO : firma
  INMUEBLE ||--o{ CONTRATO : alquilado_en
  CONTRATO ||--o{ PAGO : tiene



