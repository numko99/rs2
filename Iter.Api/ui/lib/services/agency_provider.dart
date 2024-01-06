import 'dart:convert';

import 'package:ui/models/search_result.dart';

import '../models/agency.dart';
import 'base_provider.dart';

class AgencyProvider extends BaseProvider<Agency> {
  AgencyProvider() : super("Agency");

  @override
  Agency fromJson(data) {
    return Agency.fromJson(data);
  }

  Future<SearchResult<Agency>> search([dynamic search]) async {
    var url = "$baseUrl$endpoint/search";
    if (search != null) {
      String queryString = getQueryString(search, prefix: '&');
      url = url + "?" + queryString;
    }

    var uri = Uri.parse(url);

    Map<String, String>? headers = await createHeaders();

    var response = await http!.get(uri, headers: headers);
    if (isValidResponseCode(response)) {
      var data = jsonDecode(response.body);

      return SearchResult<Agency>()
        ..count = data['count']
        ..result = data['result'].map((x) => fromJson(x)).cast<Agency>().toList();
    } else {
      throw Exception("Exception... handle this gracefully");
    }
  }
}