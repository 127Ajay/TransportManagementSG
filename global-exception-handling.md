# Global Exception Handling – Implementation Notes

## Purpose

This document contains the implementation steps followed for adding Global Exception Handling in the TransportManagementSG MVC practice project.

This document is for learning/reference purposes only.

It is NOT part of the actual application runtime.

---

# Important Decision

Since this is an ASP.NET Core MVC application:

* AJAX requests should return JSON responses
* Normal MVC page requests should redirect to an Error page

Therefore middleware was implemented to support BOTH scenarios.

---

# Project Structure Changes

## Added In Application Layer

### Folder

TransportManagementSG.Application/Exceptions

### Files

AppException.cs
NotFoundException.cs

---

## Added In UI Layer

### Folder

TransportManagementSG.UI/Middleware

### File

ExceptionMiddleware.cs
---

### Folder

TransportManagementSG.UI/Extensions

### File

MiddlewareExtensions.cs

---

# Step 1 – Created Custom Exception Classes

## AppException.cs

Purpose:

* Generic business exception
* Base custom exception

csharp
public class AppException : Exception
{
    public AppException(string message)
        : base(message)
    {
    }
}


---

## NotFoundException.cs

Purpose:

* Resource/entity not found handling

csharp
public class NotFoundException : AppException
{
    public NotFoundException(string message)
        : base(message)
    {
    }
}


---

# Step 2 – Implemented Exception Middleware

## Responsibilities

Middleware handles:

* Unhandled exceptions
* AJAX requests
* MVC requests
* Centralized logging
* Standardized responses

---

## Core Flow


Controller/Service/Repository
            ↓
Exception Thrown
            ↓
ExceptionMiddleware catches exception
            ↓
--------------------------------
AJAX Request  → JSON Response
MVC Request   → Redirect Error Page
--------------------------------


---

# Step 3 – AJAX Request Handling

Middleware checks:

csharp
request.Headers["X-Requested-With"] == "XMLHttpRequest"


If AJAX request:

Return JSON:

json
{
   "success": false,
   "message": "Something went wrong"
}


---

# Step 4 – MVC Request Handling

For non-AJAX requests:

csharp
context.Response.Redirect("/Home/Error");


MVC browser requests are redirected to:

/Home/Error

---

# Step 5 – Added Middleware Extension Method

## File

MiddlewareExtensions.cs

## Purpose

Cleaner middleware registration.

csharp
public static IApplicationBuilder UseGlobalExceptionMiddleware(
    this IApplicationBuilder app)
{
    return app.UseMiddleware<ExceptionMiddleware>();
}

---

# Step 6 – Registered Middleware In Program.cs

## Added Namespace

csharp
using TransportManagementSG.UI.Extensions;

---

## Registered Middleware

csharp
app.UseGlobalExceptionMiddleware();

---

## Placement

Middleware registered:

* AFTER `builder.Build()`
* BEFORE `MapControllerRoute()`

Correct order:

csharp
var app = builder.Build();

app.UseGlobalExceptionMiddleware();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

---

# Step 7 – AJAX Testing

## Controller Test

csharp
throw new Exception("Testing Global Exception Handler");

inside:

DeleteUser()

---

## AJAX Code Updated

Added:

javascript
error: function (xhr) {

    let response = xhr.responseJSON;

    let message = response?.message || 'User deletion failed';

    showMessage(message, 'danger');
}


---

# Step 8 – Middleware vs AJAX Understanding

## Middleware Responsibility

Server-side:

* Catch exceptions
* Prevent application crash
* Standardize responses
* Centralize logging
* Hide stack traces

---

## AJAX Responsibility

Client-side:

* Handle failed HTTP response
* Show UI message
* React to status codes

---

# Step 9 – Tested Without Middleware

Commented:

csharp
// app.UseGlobalExceptionMiddleware();


Observed:

* Uncontrolled HTML error page
* Stack traces exposed
* Inconsistent response

---

# Step 10 – Tested With Middleware

Observed:

* Clean JSON response
* Centralized handling
* Safe response structure
* Better UI handling

---

# Step 11 – Implemented MVC Error Handling

Middleware redirects:

csharp
context.Response.Redirect("/Home/Error");
---

# Step 12 – Added Error Action In HomeController

csharp
public IActionResult Error()
{
    return View();
}


---

# Step 13 – Reused Existing Shared Error View

Already available:

Views/Shared/Error.cshtml

No new error page required.

---

# Step 14 – Tested Non-AJAX MVC Request

Added:

csharp
public IActionResult TestMvcException()
{
    throw new Exception("MVC Test Exception");
}


Observed flow:


Controller Exception
        ↓
Middleware catches
        ↓
Redirects /Home/Error
        ↓
Shared Error View displayed

---

# Final Architecture

Controller
    ↓
Service
    ↓
Repository
    ↓
Exception Thrown
    ↓
Global Exception Middleware
    ↓
--------------------------------
AJAX Request  → JSON Response
MVC Request   → Error Page
--------------------------------

---

# Key Learnings

* Middleware pipeline execution
* Centralized exception handling
* MVC vs AJAX request handling
* Middleware registration order
* Extension methods
* HTTP 500 response handling
* jQuery AJAX error callbacks
* Clean architecture separation
* Enterprise MVC exception handling flow

---
