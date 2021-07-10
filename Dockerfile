# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy source
COPY ./ ./

# Build
RUN dotnet build EchoApp.sln --configuration Release

# Test
RUN dotnet test EchoApp.sln --configuration Release

# TODO make this a 2 stage docker file with seperate build and run
# TODO FROM {An appropriate runtime image}
# TODO COPY --from-build-env source desc
ENTRYPOINT ["dotnet", "run", "--project", "EchoApp/EchoApp.csproj", "--configuration", "Release"] 
