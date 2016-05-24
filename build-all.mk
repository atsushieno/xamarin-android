
MSBUILD_ARGS = 

BUILD_TOOLS = \
	build-tools/android-toolchain/android-toolchain.mdproj \
	src/Xamarin.Android.Tools.BootstrapTasks/Xmarin.Anroid.Tools.BootstrapTasks.csproj \
	build-tools/mono-runtimes/mono-runtimes.csproj \
	build-tools/jnienv-gen/jnienv-gen.csproj \
	build-tools/api-merge/api-merge.csproj

CORE = \
	external/Java.Interop/src/Java.Interop/Java.Interop.csproj \
	external/Java.Interop/tools/generator/generator.csproj \
	src/Mono.Android/Mono.Android.csproj \
	src/Mono.Android.Export/Mono.Android.Export.csproj \
	src/Xamarin.Android.Tools.Aidl/Xamarin.Android.Tools.Aidl.csproj \
	src/Xamarin.Android.Build.Tasks/Xamarin.Android.Build.Tasks.csproj \
	src/Xamarin.Android.Build.Utilities/Xamarin.Android.Build.Utilities.csproj \
	external/Java.Interop/src/Xamarin.Android.Tools.Bytecode/Xamarin.Android.Tools.Bytecode.csproj \
	src/monodroid/monodroid.mdproj \
	src/Xamarin.Android.NUnitLite/Xamarin.Android.NUnitLite.csproj

TOOLS = \
	external/Java.Interop/src/Java.Interop.Tools.JavaCallableWrappers/Java.Interop.Tools.JavaCallableWrappers.csproj \
	external/Java.Interop/tools/jcw-gen/jcw-gen.csproj \
	external/Java.Interop/src/Xamarin.Android.Tools.ApiXmlAdjuster/Xamarin.Android.Tools.ApiXmlAdjuster.csproj \
	external/Java.Interop/src/Xamarin.Android.Tools.AnnotationSupport/Xamarin.Android.Tools.AnnotationSupport.csproj \
	external/Java.Interop/src/Java.Interop.Tools.Diagnostics/Java.Interop.Tools.Diagnostics.csproj \
	external/Java.Interop/src/Java.Interop.Tools.Cecil/Java.Interop.Tools.Cecil.csproj \
	tools/api-xml-adjuster/api-xml-adjuster.csproj

TESTS = \
	src/Xamarin.Android.Build.Tasks/Tests/Xamarin.ProjectTools/Xamarin.ProjectTools.csproj \
	src/Xamarin.Android.Build.Tasks/Tests/Xamarin.Android.Build.Tests/Xamarin.Android.Build.Tests.csproj \
	src/Mono.Android/Test/Mono.Android-Tests.csproj

build-all: \
	build-build-tools \
	build-core \
	build-toolchains \
	build-tests

build-build-tools:
	for d in $(BUILD_TOOLS) ; do \
		xbuild $$d $(MSBUILD_ARGS) ; \
	done

build-core:
	for d in $(CORE) ; do \
		xbuild $$d $(MSBUILD_ARGS) ; \
	done

build-toolchains:
	for d in $(TOOLS) ; do \
		xbuild $$d $(MSBUILD_ARGS) ; \
	done
	
build-tests:
	for d in $(TESTS) ; do \
		xbuild $$d $(MSBUILD_ARGS) ; \
	done

