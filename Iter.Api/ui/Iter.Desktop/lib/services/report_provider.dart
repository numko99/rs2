import 'dart:convert';
import 'package:ui/models/arrangement_earnings.dart';
import 'package:ui/models/user_payment.dart';
import 'base_provider.dart';

class ReportProvider extends BaseProvider<dynamic, dynamic> {
  ReportProvider() : super("Report");


  Future<List<UserPaymentModel>> getUserPayments([dynamic request]) async {
    var url = "$baseUrl$endpoint/userPaymentReport";
    var uri = Uri.parse(url);

    Map<String, String>? headers = await createHeaders();

    var response =
        await http!.post(uri, headers: headers, body: jsonEncode(request));

    if (isValidResponseCode(response)) {
      var data = jsonDecode(response.body);
      return data
          .map((x) => UserPaymentModel.fromJson(x))
          .cast<UserPaymentModel>()
          .toList();
    } else {
      return [];
    }
  }

  Future<List<ArrangementEarnings>> geArrangementEarnings(
      [dynamic request]) async {
    var url = "$baseUrl$endpoint/arrangementEarnings";
    var uri = Uri.parse(url);

    Map<String, String>? headers = await createHeaders();

    var response =
        await http!.post(uri, headers: headers, body: jsonEncode(request));

    if (isValidResponseCode(response)) {
      var data = jsonDecode(response.body);
      return data
          .map((x) => ArrangementEarnings.fromJson(x))
          .cast<ArrangementEarnings>()
          .toList();
    } else {
      return [];
    }
  }
  
  // Future<void> getUserPaymentReport(String arrangementId, String? arrangementName) async {
  //   var url = Uri.parse("$baseUrl$endpoint/userPaymentReport/$arrangementId");
  //   var reportName = "Izvjestaj-${arrangementName}";
  //   await generateReport(url, reportName);
  // }

  //   Future<void> getArrangementEarningsReport(
  //     String agencyId, String? agencyName) async {
  //   var url = Uri.parse("$baseUrl$endpoint/arrangementEarnings/$agencyId");
  //   var reportName = "Izvjestaj-${agencyName}";
  //   await generateReport(url, reportName);
  // }

  // Future<void> generateReport(Uri url, String reportName) async {
  //   Map<String, String>? headers = await createHeaders();
  //   var response = await http!.post(url, headers: headers);
  //   var bytes = response.bodyBytes;
  //   var dir = await getDownloadsDirectory();
  //   String baseFilePath =
  //       '${dir!.path}${Platform.pathSeparator}$reportName';
  //   String fileExtension = '.pdf';
  //   File file = File('$baseFilePath$fileExtension');
    
  //   file = await _getUniqueFileName(file, baseFilePath, fileExtension);
    
  //   await file.writeAsBytes(bytes);
    
  //   if (await canLaunchUrl(Uri.file(file.path))) {
  //     await launchUrl(Uri.file(file.path));
  //   } else {
  //     throw 'Could not open the file';
  //   }
  // }

  // Future<File> _getUniqueFileName(
  //     File file, String baseFilePath, String extension) async {
  //   int counter = 1;
  //   while (await file.exists()) {
  //     file = File('$baseFilePath ($counter)$extension');
  //     counter++;
  //   }
  //   return file;
  // }
}
