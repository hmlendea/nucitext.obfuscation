# Privacy and Personal Data

NuciText.Obfuscation is a pure .NET text-transformation library with no network access, telemetry, or data collection. The library performs in-memory obfuscation and deobfuscation only. Where the library is used in an application, the application operator controls any personal data processed through the library.

## 📑 Table of Contents

- [What This Document Covers](#what-this-document-covers)
- [Self-Hosted Deployments](#self-hosted-deployments)
- [Data We Handle](#data-we-handle)
  - [Data Provided to the Application](#data-provided-to-the-application)
  - [Data Generated or Collected by the Application](#data-generated-or-collected-by-the-application)
  - [Data Received from Integrations](#data-received-from-integrations)
- [Processing and Use](#processing-and-use)
- [Storage, Retention, and Deletion](#storage-retention-and-deletion)
- [External Processing and Integrations](#external-processing-and-integrations)
- [User Controls and Requests](#user-controls-and-requests)
- [Data Protection and Security](#data-protection-and-security)
- [Document Changes](#document-changes)
- [Contact](#contact)

## 🔎 What This Document Covers

This document describes how NuciText.Obfuscation at https://github.com/hmlendea/nucitext.obfuscation handles personal data. It covers the library behaviour and verified integrations described below. Where the software is self-hosted, the instance operator may have separate responsibilities described below.

## 🏠 Self-Hosted Deployments

NuciText.Obfuscation is a .NET library distributed via NuGet. It can be included in any application deployed and operated by users or organisations. This document covers the library maintainers' practices; self-hosted instance operators control their application's configuration, local storage, logs, backups, access controls, retention, and request handling unless the library directly controls those functions.

The library itself sends no data from a self-hosted instance to project maintainers or external services. There is no telemetry, update checks, crash reports, email, authentication, reverse-proxy, object-storage, or monitoring integrations built into the library.

## 📥 Data We Handle

### Data Provided to the Application

The library does not request or require any personal data. Applications using the library may pass text strings to the `Obfuscate` and `Deobfuscate` methods; any personal data in those strings is provided by the application, not requested by the library.

### Data Generated or Collected by the Application

The library generates no server, application, access, audit, telemetry, crash, or update-check data. It performs only in-memory text transformation and returns the result to the caller.

### Data Received from Integrations

The library has no built-in integrations and receives no personal data from third parties.

## 🧭 Processing and Use

The library processes the data described above for these verified functions:
- Text obfuscation — Input text strings passed to `Obfuscate`
- Text deobfuscation — Input text strings passed to `Deobfuscate`

## 🗄️ Storage, Retention, and Deletion

The library stores no data. All transformations occur in memory; the library writes nothing to local databases, files, logs, caches, backups, or configured storage services. For self-hosted deployments, the instance operator controls storage, deletion, and backups of any application data that passes through the library.

## 🔗 External Processing and Integrations

The library has no built-in external data transfer. It does not communicate with any external services, recipients, or integrations.

| Service or integration | Purpose | Data involved | Configuration or documentation |
|-----------------------|---------|---------------|--------------------------------|
| None | Not applicable | Not applicable | Not applicable |

## ⚙️ User Controls and Requests

The following documented controls or request procedures are available:
- Application operators control all input passed to the library and all output received from it — Use the library's public API (`Obfuscate`, `Deobfuscate`) with any text; no library-level controls exist because the library retains no data.

## 🛡️ Data Protection and Security

The library performs deterministic in-memory text transformations using static replacement tables. It uses no cryptography, secrets, or network resources. For self-hosted deployments, the operator is responsible for application-level updates, secrets management, access controls, backups, network exposure, and log protection. The library itself introduces no additional attack surface beyond in-memory string manipulation.

The original maintainers are not responsible for changes, modifications, or privacy issues introduced by downstream forks.

## 🔄 Document Changes

Update this document when application data flows, storage, integrations, or deployment responsibilities change. The current version is published at https://github.com/hmlendea/nucitext.obfuscation/blob/main/PRIVACY.md.

## 📬 Contact

For questions about library data handling, contact the project maintainers via GitHub issues at https://github.com/hmlendea/nucitext.obfuscation/issues. For a self-hosted application using the library, contact the application operator, unless the project explicitly handles the request. Include the application name and deployment context if applicable; do not send passwords, access tokens, or other secrets.