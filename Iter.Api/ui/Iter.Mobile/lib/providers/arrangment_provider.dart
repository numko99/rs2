import 'dart:convert';

import 'package:iter_mobile/models/arrangement_price.dart';
import 'package:iter_mobile/models/arrangement_search_response.dart';
import 'package:iter_mobile/models/arrangment.dart';

import 'base_provider.dart';

class ArrangmentProvider extends BaseProvider<Arrangement, ArrangementSearchResponse> {
  ArrangmentProvider() : super("Arrangement");

  @override
  Arrangement fromJson(data) {
    return Arrangement.fromJson(data);
  }

   @override
  ArrangementSearchResponse fromJsonSearch(data) {
    return ArrangementSearchResponse.fromJson(data);
  }

  Future<ArrangementPrice> getArrangementPrice(var arrangementId) async {
    var url = Uri.parse("$baseUrl$endpoint/arrangementPrice/$arrangementId");

    Map<String, String>? headers = await createHeaders();

    var response = await http!.get(url, headers: headers);
    if (isValidResponseCode(response)) {
      var data = jsonDecode(response.body);
      return ArrangementPrice.fromJson(data) as ArrangementPrice;
    } else {
      throw Exception("Exception... handle this gracefully");
    }
  }

  Future<void> changeStatus(var arrangementId, var arrangementStatusId) async {
    var url = Uri.parse("$baseUrl$endpoint/changeStatus/$arrangementId");

    Map<String, String>? headers = await createHeaders();

    await http!.put(url, headers: headers, body: jsonEncode(arrangementStatusId));
  }

    Future<List<ArrangementSearchResponse>> getRecommendedArrangements(var arrangementId) async {
    var url = Uri.parse("$baseUrl$endpoint/recommendedArrangements/$arrangementId");

    Map<String, String>? headers = await createHeaders();

    var response = await http!.get(url, headers: headers);
    if (isValidResponseCode(response)) {
      var data = jsonDecode(response.body);
      return data.map((x) => fromJsonSearch(x)).cast<ArrangementSearchResponse>().toList();
    } else {
      throw Exception("Exception... handle this gracefully");
    }
  }
}