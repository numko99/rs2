import 'dart:convert';

import 'package:ui/models/arrangement_price.dart';
import 'package:ui/models/arrangment.dart';
import 'base_provider.dart';

class ArrangmentProvider extends BaseProvider<Arrangement> {
  ArrangmentProvider() : super("Arrangement");

  @override
  Arrangement fromJson(data) {
    return Arrangement.fromJson(data);
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

}