```mermaid
erDiagram
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
