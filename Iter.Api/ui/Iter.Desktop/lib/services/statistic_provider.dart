import 'dart:convert';

import 'package:ui/models/admin_statistc_response.dart';
import 'base_provider.dart';

class StatisticProvider
    extends BaseProvider<AdminStatisticResponse, AdminStatisticResponse> {
  StatisticProvider() : super("Home");

  @override
  AdminStatisticResponse fromJson(data) {
    return AdminStatisticResponse.fromJson(data);
  }

  Future<AdminStatisticResponse?> getAvailableStatistics() async {
    var url = Uri.parse("$baseUrl$endpoint/adminStatistic");

    Map<String, String>? headers = await createHeaders();
    var response = await http!.get(url, headers: headers);

    if (isValidResponseCode(response)) {
      var data = jsonDecode(response.body);
      return fromJson(data);
    } else {
      return null;
    }
  }

    Future<AdminStatisticResponse?> getEmployeeStatistics(agencyId) async {
    var url = Uri.parse("$baseUrl$endpoint/employeeStatistic?agencyId=$agencyId");

    Map<String, String>? headers = await createHeaders();
    var response = await http!.get(url, headers: headers);

    if (isValidResponseCode(response)) {
      var data = jsonDecode(response.body);
      return fromJson(data);
    } else {
      return null;
    }
  }
}
