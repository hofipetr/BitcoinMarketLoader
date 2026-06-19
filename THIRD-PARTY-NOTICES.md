# Third-party notices

BitcoinMarketLoader includes or distributes third-party software. The project
license in `LICENSE` applies only to BitcoinMarketLoader's own code. Third-party
components remain subject to their respective licenses.

## MIT-licensed components

- Bootstrap 5.3.8 — Copyright (c) 2011-2025 The Bootstrap Authors.
  The complete license is also retained at
  `BitcoinMarketLoader.Web/wwwroot/lib/bootstrap/LICENSE`.
- Popper 2.11.8 — Copyright (c) 2019 Federico Zivolo. Popper is included in
  Bootstrap's bundled JavaScript.
- Microsoft .NET, ASP.NET Core, Entity Framework Core, Microsoft.Data.SqlClient,
  Microsoft.OpenApi, Azure SDK, Microsoft.IdentityModel, and related runtime
  libraries — Copyright Microsoft Corporation and contributors.
- Swashbuckle.AspNetCore 10.2.1 — Copyright its contributors.
- Humanizer.Core, Newtonsoft.Json, and other transitive MIT-licensed runtime
  libraries — Copyright their respective authors and contributors.

The MIT license terms applicable to these components are:

> Permission is hereby granted, free of charge, to any person obtaining a copy
> of this software and associated documentation files (the "Software"), to deal
> in the Software without restriction, including without limitation the rights
> to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
> copies of the Software, and to permit persons to whom the Software is
> furnished to do so, subject to the following conditions:
>
> The above copyright notice and this permission notice shall be included in all
> copies or substantial portions of the Software.
>
> THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
> IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
> FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
> AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
> LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
> OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
> SOFTWARE.

## NLog — BSD licenses

- NLog 6.1.3 — BSD-3-Clause, Copyright (c) 2004-2026 NLog Project.
- NLog.Extensions.Logging 6.1.3 — BSD-2-Clause,
  Copyright (c) 2004-2026 NLog Project.
- NLog.Web.AspNetCore 6.1.3 — BSD-3-Clause,
  Copyright (c) 2015-2026 NLog Project.

BSD-2-Clause terms:

> Redistribution and use in source and binary forms, with or without
> modification, are permitted provided that the following conditions are met:
>
> 1. Redistributions of source code must retain the above copyright notice,
> this list of conditions and the following disclaimer.
> 2. Redistributions in binary form must reproduce the above copyright notice,
> this list of conditions and the following disclaimer in the documentation
> and/or other materials provided with the distribution.
>
> THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
> AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
> IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
> ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
> LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
> CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
> SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
> INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
> CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
> ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
> POSSIBILITY OF SUCH DAMAGE.

BSD-3-Clause adds this condition:

> Neither the name of the copyright holder nor the names of its contributors
> may be used to endorse or promote products derived from this software without
> specific prior written permission.

## Microsoft.Data.SqlClient.SNI.runtime

Microsoft.Data.SqlClient.SNI.runtime 6.0.2 is distributed under separate
Microsoft software license terms. A complete copy is included at
`ThirdPartyLicenses/Microsoft.Data.SqlClient.SNI.runtime.txt`.

## Development and test dependencies

The source tree also references development-only packages such as Sass,
Mapperly, xUnit, NSubstitute, coverlet, and their transitive dependencies.
Their license identifiers and exact versions are recorded in
`BitcoinMarketLoader.Web/package-lock.json` and the NuGet project files.
These packages are not included as standalone components in the application
publish output.
