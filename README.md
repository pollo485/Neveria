# 🍦 Freeze Dream — Nevería Web App

> Sistema web para la gestión de una nevería, desarrollado con **ASP.NET Core MVC** y **Bootstrap 5**.

---

## 📋 Descripción

**Freeze Dream** es una aplicación web de gestión para una nevería que permite administrar productos, registrar ventas y visualizar estadísticas gráficas. El proyecto fue desarrollado como una solución MVC completa con vistas estilizadas, navegación dinámica y una interfaz amigable con temática de helados.

---

## 🚀 Tecnologías utilizadas

| Tecnología | Versión | Uso |
|---|---|---|
| ASP.NET Core MVC | .NET 8 | Framework principal |
| Bootstrap | 5.3.2 | Estilos y componentes UI |
| Chart.js | 4.4.1 | Gráficas de ventas |
| Google Fonts | — | Fredoka One + Nunito |
| CSS personalizado | — | Estilos por vista |

---

## 📁 Estructura del proyecto

```
Neveria/
├── Controllers/
│   └── HomeController.cs         # Controlador principal con todas las acciones
│
├── Models/
│   └── ErrorViewModel.cs
│
├── Views/
│   └── Home/
│       ├── Login.cshtml           # Página de inicio de sesión
│       ├── Registro.cshtml        # Registro de nuevos usuarios
│       ├── Inicio.cshtml          # Página principal / Home
│       ├── Productos.cshtml       # Compra de productos con carrito
│       ├── DetallesProducto.cshtml# Edición de detalles de productos
│       ├── Ventas.cshtml          # Registro y listado de ventas
│       └── Graficos.cshtml        # Estadísticas y gráficas
│   └── Shared/
│       └── _Layout.cshtml         # Layout compartido con navbar
│
├── wwwroot/
│   └── css/
│       ├── home.css
│       ├── login.css
│       ├── registro.css
│       ├── productos.css
│       ├── detalles-producto.css
│       ├── ventas.css
│       └── graficos.css
│
├── Program.cs                     # Configuración y rutas
└── appsettings.json
```

---

## 🖥️ Vistas principales

### 🔐 Login
Pantalla de inicio de sesión con fondo durazno, tarjeta rosa y decoraciones animadas.

### 👤 Registro
Formulario para crear nuevos usuarios. Valida que las contraseñas coincidan.

### 🏠 Inicio
Página principal con diseño split (izquierda durazno / derecha rosa) y tarjetas de características.

### 🛒 Productos
Lista de productos con buscador y carrito interactivo en tiempo real.

### ✏️ Detalles de Producto
Panel de dos columnas: lista de productos a la izquierda y formulario de edición a la derecha.

### 💳 Ventas
Tabla de ventas con badges de estado (Completada / Pendiente / Cancelada).

### 📊 Detalles Gráficos
Dashboard con estadísticas rápidas, gráfica de pastel, gráfica de barras y tabla de ventas recientes.

---

## ⚙️ Instalación y ejecución

### Requisitos previos
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022 o Visual Studio Code

### Pasos

1. **Clona el repositorio**
   ```bash
   git clone https://github.com/tu-usuario/freeze-dream.git
   cd freeze-dream
   ```

2. **Restaura las dependencias**
   ```bash
   dotnet restore
   ```

3. **Ejecuta el proyecto**
   ```bash
   dotnet run
   ```

4. **Abre en el navegador**
   ```
   https://localhost:7230
   ```

   > La página de inicio es el **Login**. Puedes ingresar cualquier usuario y contraseña para acceder.

---

## 🗺️ Rutas disponibles

| Ruta | Vista | Descripción |
|---|---|---|
| `/Home/Login` | Login | Inicio de sesión |
| `/Home/Registro` | Registro | Crear cuenta nueva |
| `/Home/Inicio` | Inicio | Página principal |
| `/Home/Productos` | Productos | Comprar helados |
| `/Home/DetallesProducto` | Detalles | Editar productos |
| `/Home/Ventas` | Ventas | Ver registro de ventas |
| `/Home/Graficos` | Gráficos | Estadísticas y reportes |

---

## 🎨 Paleta de colores

| Color | Hex | Uso |
|---|---|---|
| 🍑 Durazno | `#F5C97A` | Fondo Home / Login |
| 🌸 Rosa | `#F4607A` | Tarjetas, botones primarios |
| 🩵 Cielo | `#87CEEB` | Navbar, inputs Login |
| 🟫 Café oscuro | `#3E2723` | Fondo Productos |
| 🟢 Verde | `#5D8A4E` | Tarjetas de productos |
| 🟣 Morado | `#7B3FE4` | Fondo Gráficos |
| 🤍 Crema | `#FFF8F0` | Fondo Ventas / Detalles |

---

## 📌 Notas

- Los datos de productos y ventas son de **demostración** (hardcodeados en JS/HTML).
- El login y registro son **visuales**, pendientes de conectar a base de datos.
- El botón "Guardar cambios" en Detalles de Producto es visual, pendiente de backend.

---

## Autor

Desarrollado por **Jesus Alonso Valenzuela Armenta** — Proyecto escolar de nevería *Freeze Dream* 🍦

---

> _"Los helados más deliciosos de la ciudad"_ 🍧
